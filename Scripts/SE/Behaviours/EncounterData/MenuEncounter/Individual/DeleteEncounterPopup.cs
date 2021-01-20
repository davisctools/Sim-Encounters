using ClinicalTools.UI;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class DeleteEncounterPopup : MonoBehaviour, IDeleteEncounterHandler
    {
        public Button LocalButton { get => localButton; set => localButton = value; }
        [SerializeField] private Button localButton;
        public Button ServerButton { get => serverButton; set => serverButton = value; }
        [SerializeField] private Button serverButton;
        public Button BothButton { get => bothButton; set => bothButton = value; }
        [SerializeField] private Button bothButton;

        protected ISelectedListener<MenuSceneInfoSelectedEventArgs> SceneInfoSelectedListener { get; set; }
        protected IEncounterRemover LocalEncounterRemover { get; set; }
        protected IEncounterRemover ServerEncounterRemover { get; set; }
        protected BaseConfirmationPopup ConfirmationPopup { get; set; }

        [Inject]
        public virtual void Inject(
            ISelectedListener<MenuSceneInfoSelectedEventArgs> sceneInfoSelectedListener,
            [Inject(Id = SaveType.Local)] IEncounterRemover localEncounterRemover,
            [Inject(Id = SaveType.Server)] IEncounterRemover serverEncounterRemover,
            BaseConfirmationPopup confirmationPopup)
        {
            SceneInfoSelectedListener = sceneInfoSelectedListener;
            LocalEncounterRemover = localEncounterRemover;
            ServerEncounterRemover = serverEncounterRemover;
            ConfirmationPopup = confirmationPopup;
        }

        protected virtual void Start()
        {
            LocalButton.onClick.AddListener(ConfirmDeletingLocal);
            ServerButton.onClick.AddListener(ConfirmDeletingServer);
            BothButton.onClick.AddListener(ConfirmDeletingBoth);
        }

        protected WaitableTask DeleteTask { get; set; }
        protected User User { get; set; }
        protected MenuEncounter Encounter { get; set; }
        public virtual WaitableTask Delete(User user, MenuEncounter encounter)
        {
            User = user;
            Encounter = encounter;
            var canDeleteLocal = encounter.Metadata.ContainsKey(SaveType.Local) || encounter.Metadata.ContainsKey(SaveType.Autosave);
            var canDeleteServer = encounter.Metadata.ContainsKey(SaveType.Server);

            DeleteTask = new WaitableTask();
            if (canDeleteLocal && canDeleteServer)
                gameObject.SetActive(true);
            else if (canDeleteServer)
                ConfirmDeletingServer();
            else if (canDeleteLocal)
                ConfirmDeletingLocal();
            else
                return new WaitableTask(new Exception("Cannot delete case"));

            return DeleteTask;
        }

        protected virtual void ConfirmDeletingBoth()
            => ConfirmationPopup.ShowConfirmation(DeleteBothConfirmed, Cancel, "Confirm Deletion", "Are you sure you want to delete this case from your computer and the server?");
        protected virtual void DeleteBothConfirmed()
        {
            var localDeleteTask = DeleteLocalEncounter();
            var serverDeleteTask = DeleteServerEncounter();

            SetTaskOnceOtherTasksAreCompleted(DeleteTask, localDeleteTask, serverDeleteTask);

            gameObject.SetActive(false);
        }


        protected virtual void ConfirmDeletingLocal()
            => ConfirmationPopup.ShowConfirmation(DeleteLocalConfirmed, Cancel, "Confirm Deletion", "Are you sure you want to delete this case from your computer?");
        protected virtual void DeleteLocalConfirmed()
        {
            var localDeleteTask = DeleteLocalEncounter();
            localDeleteTask.CopyValueWhenCompleted(DeleteTask);
            gameObject.SetActive(false);
        }

        protected virtual void ConfirmDeletingServer()
            => ConfirmationPopup.ShowConfirmation(DeleteServerConfirmed, Cancel, "Confirm Deletion", "Are you sure you want to delete this case from the server?");
        protected virtual void DeleteServerConfirmed()
        {
            var serverDeleteTask = DeleteServerEncounter();
            serverDeleteTask.CopyValueWhenCompleted(DeleteTask);
            gameObject.SetActive(false);
        }

        protected virtual void SetTaskOnceOtherTasksAreCompleted(WaitableTask taskToSet, WaitableTask taskToCheck1, WaitableTask taskToCheck2)
        {
            taskToCheck1.AddOnCompletedListener((action) => SetTaskIfOtherTasksAreCompleted(taskToSet, taskToCheck1, taskToCheck2));
            taskToCheck2.AddOnCompletedListener((action) => SetTaskIfOtherTasksAreCompleted(taskToSet, taskToCheck1, taskToCheck2));
        }
        protected virtual void SetTaskIfOtherTasksAreCompleted(WaitableTask taskToSet, WaitableTask taskToCheck1, WaitableTask taskToCheck2)
        {
            if (!taskToCheck1.IsCompleted() || !taskToCheck2.IsCompleted())
                return;

            if (taskToCheck1.Result.IsError())
                taskToSet.SetError(taskToCheck1.Result.Exception);
            else if (taskToCheck2.Result.IsError())
                taskToSet.SetError(taskToCheck2.Result.Exception);
            else
                taskToSet.SetCompleted();
        }

        protected virtual WaitableTask DeleteLocalEncounter()
        {
            var localDeleteTask = DeleteLocalEncounter(SaveType.Autosave);
            var autosaveDeleteTask = DeleteLocalEncounter(SaveType.Local);

            if (autosaveDeleteTask == null)
                return localDeleteTask;
            else if (localDeleteTask == null)
                return autosaveDeleteTask;

            var deleteTask = new WaitableTask();
            SetTaskOnceOtherTasksAreCompleted(deleteTask, localDeleteTask, autosaveDeleteTask);
            return deleteTask;
        }
        protected virtual WaitableTask DeleteLocalEncounter(SaveType saveType)
        {
            if (!Encounter.Metadata.ContainsKey(saveType))
                return null;

            var localDeleteTask = LocalEncounterRemover.Delete(User, Encounter.Metadata[saveType]);
            return RemoveMetadataOnDeleted(Encounter, localDeleteTask, saveType);
        }
        protected virtual WaitableTask DeleteServerEncounter()
        {
            var serverDeleteTask = ServerEncounterRemover.Delete(User, Encounter.Metadata[SaveType.Server]);
            return RemoveMetadataOnDeleted(Encounter, serverDeleteTask, SaveType.Server);
        }

        protected virtual WaitableTask RemoveMetadataOnDeleted(MenuEncounter encounter, WaitableTask deleteTask, SaveType saveType)
        {
            var task = new WaitableTask();
            deleteTask.AddOnCompletedListener((result) => RemoveMetadataIfNoError(task, encounter, result, saveType));
            return task;
        }

        protected virtual void RemoveMetadataIfNoError(WaitableTask task, MenuEncounter encounter, TaskResult deleteResult, SaveType saveType)
        {
            if (deleteResult.IsError()) {
                task.SetError(deleteResult.Exception);
                return;
            }

            Encounter.Metadata.Remove(saveType);

            if (Encounter.Metadata.Count == 0)
                SceneInfoSelectedListener.CurrentValue.SceneInfo.MenuEncountersInfo.RemoveEncounter(encounter);
            task.SetCompleted();
        }

        public virtual void Cancel()
        {
            DeleteTask.SetError(new Exception("Cancelled"));
            gameObject.SetActive(false);
        }
    }
}
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

        [Inject]
        public virtual void Inject(
            ISelectedListener<MenuSceneInfoSelectedEventArgs> sceneInfoSelectedListener,
            [Inject(Id = SaveType.Local)] IEncounterRemover localEncounterRemover,
            [Inject(Id = SaveType.Server)] IEncounterRemover serverEncounterRemover)
        {
            SceneInfoSelectedListener = sceneInfoSelectedListener;
            LocalEncounterRemover = localEncounterRemover;
            ServerEncounterRemover = serverEncounterRemover;
        }

        protected virtual void Start()
        {
            LocalButton.onClick.AddListener(DeleteLocalButtonPressed);
            ServerButton.onClick.AddListener(DeleteServerButtonPressed);
            BothButton.onClick.AddListener(DeleteBothButton);
        }

        protected WaitableTask DeleteTask { get; set; }
        protected User User { get; set; }
        protected MenuEncounter Encounter { get; set; }
        protected bool CanDeleteLocal { get; set; }
        protected bool CanDeleteServer { get; set; }
        public virtual WaitableTask Delete(User user, MenuEncounter encounter)
        {
            gameObject.SetActive(true);

            User = user;
            Encounter = encounter;
            CanDeleteLocal = encounter.Metadata.ContainsKey(SaveType.Local);
            CanDeleteServer = encounter.Metadata.ContainsKey(SaveType.Server);

            LocalButton.gameObject.SetActive(CanDeleteLocal);
            ServerButton.gameObject.SetActive(CanDeleteServer);
            BothButton.gameObject.SetActive(CanDeleteLocal && CanDeleteServer);

            return DeleteTask = new WaitableTask();
        }

        protected virtual void DeleteBothButton()
        {
            var localDeleteTask = DeleteLocalEncounter();
            var serverDeleteTask = DeleteServerEncounter();

            localDeleteTask.AddOnCompletedListener((action) => SetDeleteTaskAfterDeletingBoth(localDeleteTask, serverDeleteTask));
            serverDeleteTask.AddOnCompletedListener((action) => SetDeleteTaskAfterDeletingBoth(localDeleteTask, serverDeleteTask));

            gameObject.SetActive(false);
        }

        protected virtual void DeleteLocalButtonPressed()
        {
            var localDeleteTask = DeleteLocalEncounter();
            localDeleteTask.CopyValueWhenCompleted(DeleteTask);
            gameObject.SetActive(false);
        }

        protected virtual void DeleteServerButtonPressed()
        {
            var serverDeleteTask = DeleteServerEncounter();
            serverDeleteTask.CopyValueWhenCompleted(DeleteTask);
            gameObject.SetActive(false);
        }

        protected virtual void SetDeleteTaskAfterDeletingBoth(WaitableTask localDeleteTask, WaitableTask serverDeleteTask)
        {
            if (!localDeleteTask.IsCompleted() || !serverDeleteTask.IsCompleted())
                return;

            if (localDeleteTask.Result.IsError())
                DeleteTask.SetError(localDeleteTask.Result.Exception);
            else if (serverDeleteTask.Result.IsError())
                DeleteTask.SetError(serverDeleteTask.Result.Exception);
            else
                DeleteTask.SetCompleted();
        }

        protected virtual WaitableTask DeleteLocalEncounter()
        {
            var localDeleteTask = LocalEncounterRemover.Delete(User, Encounter.Metadata[SaveType.Local]);
            RemoveMetadataOnDeleted(Encounter, localDeleteTask, SaveType.Local);
            return localDeleteTask;
        }
        protected virtual WaitableTask DeleteServerEncounter()
        {
            var serverDeleteTask = ServerEncounterRemover.Delete(User, Encounter.Metadata[SaveType.Server]);
            RemoveMetadataOnDeleted(Encounter, serverDeleteTask, SaveType.Server);
            return serverDeleteTask;
        }

        protected virtual void RemoveMetadataOnDeleted(MenuEncounter encounter, WaitableTask deleteTask, SaveType saveType)
            => deleteTask.AddOnCompletedListener((result) => RemoveMetadataIfNoError(encounter, result, saveType));
        protected virtual void RemoveMetadataIfNoError(MenuEncounter encounter, TaskResult deleteResult, SaveType saveType)
        {
            if (deleteResult.IsError())
                return;
            Encounter.Metadata.Remove(saveType);

            if (Encounter.Metadata.Count == 0)
                SceneInfoSelectedListener.CurrentValue.SceneInfo.MenuEncountersInfo.RemoveEncounter(encounter);
        }

        public virtual void Cancel()
        {
            DeleteTask.SetError(new Exception("Cancelled"));
            gameObject.SetActive(false);
        }
    }
}
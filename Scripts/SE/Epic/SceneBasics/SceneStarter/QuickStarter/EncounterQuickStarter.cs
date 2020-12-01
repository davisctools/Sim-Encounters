using System;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class EncounterQuickStarter : IEncounterQuickStarter
    {
        protected IMenuEncountersInfoReader MenuEncountersReader { get; set; }
        protected IReaderSceneStarter SceneStarter { get; set; }
        protected IUserEncounterReader EncounterReader { get; set; }

        [Inject]
        public virtual void Inject(IMenuEncountersInfoReader menuInfoReader, IReaderSceneStarter sceneStarter, IUserEncounterReader encounterReader)
        {
            MenuEncountersReader = menuInfoReader;
            SceneStarter = sceneStarter;
            EncounterReader = encounterReader;
        }

        public void StartEncounter(User user, ILoadingScreen loadingScreen, int recordNumber)
        {
            var encounters = MenuEncountersReader.GetMenuEncountersInfo(user);
            StartEncounter(user, loadingScreen, encounters, recordNumber);
        }

        public void StartEncounter(User user, ILoadingScreen loadingScreen, WaitableTask<IMenuEncountersInfo> encounters, int recordNumber)
        {
            loadingScreen.Show();
            var encounter = GetEncounter(user, encounters, recordNumber);
            var loadingSceneInfo = new LoadingReaderSceneInfo(user, loadingScreen, encounter);
            SceneStarter.StartScene(loadingSceneInfo);
        }

        protected virtual WaitableTask<UserEncounter> GetEncounter(User user, WaitableTask<IMenuEncountersInfo> encounters, int recordNumber)
        {
            var result = new WaitableTask<UserEncounter>();
            encounters.AddOnCompletedListener((encountersResult) => SetUserEncounter(result, user, encountersResult, recordNumber));

            return result;
        }

        protected virtual void SetUserEncounter(WaitableTask<UserEncounter> result, User user, TaskResult<IMenuEncountersInfo> encounters, int recordNumber)
        {
            if (encounters.IsError()) {
                result.SetError(encounters.Exception);
                return;
            }

            foreach (var encounter in encounters.Value.GetEncounters()) {
                var typedMetadata = encounter.GetLatestTypedMetada();
                if (typedMetadata.Value.RecordNumber != recordNumber)
                    continue;

                var userEncounter = EncounterReader.GetUserEncounter(user, typedMetadata.Value, encounter.Status, typedMetadata.Key);
                userEncounter.CopyValueWhenCompleted(result);
                return;
            }

            result.SetError(new Exception("Could not find an encounter with the given record number."));
        }
    }
}
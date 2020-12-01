
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public class LoadingReaderSceneInfo
    {
        public User User { get; }
        public ILoadingScreen LoadingScreen { get; }
        public WaitableTask<UserEncounter> Encounter { get; }
        public List<MenuEncounter> SuggestedEncounters { get; } = new List<MenuEncounter>();

        public WaitableTask<ReaderSceneInfo> Result { get; } = new WaitableTask<ReaderSceneInfo>();

        public LoadingReaderSceneInfo(User user, ILoadingScreen loadingScreen, WaitableTask<UserEncounter> encounter)
        {
            User = user;
            LoadingScreen = loadingScreen;
            Encounter = encounter;
            Encounter.AddOnCompletedListener(EncounterRetrieved);
        }

        private void EncounterRetrieved(TaskResult<UserEncounter> encounter)
        {
            var loadedInfo = new ReaderSceneInfo(this);
            Result.SetResult(loadedInfo);
        }
    }
}
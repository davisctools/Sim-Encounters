namespace ClinicalTools.SimEncounters
{
    public class LoadingWriterSceneInfo
    {
        public User User { get; }
        public ILoadingScreen LoadingScreen { get; }
        public WaitableTask<Encounter> Encounter { get; }

        public WaitableTask<WriterSceneInfo> Result { get; } = new WaitableTask<WriterSceneInfo>();

        public LoadingWriterSceneInfo(User user, ILoadingScreen loadingScreen, WaitableTask<Encounter> encounter)
        {
            User = user;
            LoadingScreen = loadingScreen;
            Encounter = encounter;
            Encounter.AddOnCompletedListener(EncounterRetrieved);
        }

        private void EncounterRetrieved(TaskResult<Encounter> encounter)
        {
            var loadedInfo = new WriterSceneInfo(this);
            Result.SetResult(loadedInfo);
        }
    }
}
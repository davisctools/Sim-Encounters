namespace ClinicalTools.SimEncounters
{
    public class WriterSceneInfo
    {
        public User User { get; }
        public ILoadingScreen LoadingScreen { get; }
        public ContentEncounter Encounter { get; }

        public WriterSceneInfo(LoadingWriterSceneInfo loadingEncounterSceneInfo)
        {
            User = loadingEncounterSceneInfo.User;
            LoadingScreen = loadingEncounterSceneInfo.LoadingScreen;
            if (!loadingEncounterSceneInfo.Encounter.Result.IsError())
                Encounter = loadingEncounterSceneInfo.Encounter.Result.Value;
        }
    }
}
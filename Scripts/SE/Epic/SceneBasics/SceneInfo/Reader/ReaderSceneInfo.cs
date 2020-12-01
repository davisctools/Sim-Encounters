using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public class ReaderSceneInfo
    {
        public User User { get; }
        public ILoadingScreen LoadingScreen { get; }
        public UserEncounter Encounter { get; }
        public List<MenuEncounter> SuggestedEncounters { get; } = new List<MenuEncounter>();

        public ReaderSceneInfo(LoadingReaderSceneInfo loadingEncounterSceneInfo)
        {
            User = loadingEncounterSceneInfo.User;
            LoadingScreen = loadingEncounterSceneInfo.LoadingScreen;
            Encounter = loadingEncounterSceneInfo.Encounter.Result.Value;
            SuggestedEncounters = loadingEncounterSceneInfo.SuggestedEncounters;
        }
    }
}
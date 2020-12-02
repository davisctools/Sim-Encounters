namespace ClinicalTools.SimEncounters
{
    public class MenuSceneInfo
    {
        public User User { get; }
        public ILoadingScreen LoadingScreen { get; }
        public IMenuEncountersInfo MenuEncountersInfo { get; }

        public MenuSceneInfo(LoadingMenuSceneInfo loadingMenuSceneInfo)
        {
            User = loadingMenuSceneInfo.User;
            LoadingScreen = loadingMenuSceneInfo.LoadingScreen;
            MenuEncountersInfo = loadingMenuSceneInfo.MenuEncountersInfo.Result.Value;
        }
    }
}
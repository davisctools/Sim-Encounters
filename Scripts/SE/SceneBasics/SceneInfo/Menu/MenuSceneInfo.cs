namespace ClinicalTools.SimEncounters
{
    public enum MenuArea { InitialSelection, Cases }

    public class MenuSceneInfo
    {
        public User User { get; }
        public MenuArea MenuArea { get; }
        public ILoadingScreen LoadingScreen { get; }
        public IMenuEncountersInfo MenuEncountersInfo { get; }

        public MenuSceneInfo(LoadingMenuSceneInfo loadingMenuSceneInfo)
        {
            User = loadingMenuSceneInfo.User;
            LoadingScreen = loadingMenuSceneInfo.LoadingScreen;
            MenuArea = loadingMenuSceneInfo.MenuArea;
            MenuEncountersInfo = loadingMenuSceneInfo.MenuEncountersInfo.Result.Value;
        }
    }
}
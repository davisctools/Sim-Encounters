namespace ClinicalTools.SimEncounters
{
    public class LoadingMenuSceneInfo
    {
        public User User { get; }
        public ILoadingScreen LoadingScreen { get; }
        public MenuArea MenuArea { get; }
        public WaitableTask<IMenuEncountersInfo> MenuEncountersInfo { get; }
        public WaitableTask<MenuSceneInfo> Result = new WaitableTask<MenuSceneInfo>();

        public LoadingMenuSceneInfo(User user, ILoadingScreen loadingScreen, MenuArea menuArea, WaitableTask<IMenuEncountersInfo> menuEncountersInfo)
        {
            User = user;
            LoadingScreen = loadingScreen;
            MenuArea = menuArea;
            MenuEncountersInfo = menuEncountersInfo;
            MenuEncountersInfo.AddOnCompletedListener(CategoriesRetrieved);
        }

        private void CategoriesRetrieved(TaskResult<IMenuEncountersInfo> menuEncountersInfo)
        {
            var loadedInfo = new MenuSceneInfo(this);
            Result.SetResult(loadedInfo);
        }
    }
}
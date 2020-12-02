namespace ClinicalTools.SimEncounters
{
    public class LoadingMenuSceneInfo
    {
        public User User { get; }
        public ILoadingScreen LoadingScreen { get; }
        public WaitableTask<IMenuEncountersInfo> MenuEncountersInfo { get; }
        public WaitableTask<MenuSceneInfo> Result = new WaitableTask<MenuSceneInfo>();

        public LoadingMenuSceneInfo(User user, ILoadingScreen loadingScreen, WaitableTask<IMenuEncountersInfo> menuEncountersInfo)
        {
            User = user;
            LoadingScreen = loadingScreen;
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
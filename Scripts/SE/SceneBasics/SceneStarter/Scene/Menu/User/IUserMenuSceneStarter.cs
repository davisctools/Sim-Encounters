namespace ClinicalTools.SimEncounters
{
    public interface IUserMenuSceneStarter
    {
        void StartMenuScene(User user, ILoadingScreen loadingScreen, MenuArea menuArea);
        void ConfirmStartingMenuScene(User user, ILoadingScreen loadingScreen, MenuArea menuArea);
    }
}

namespace ClinicalTools.SimEncounters
{
    public interface IUserMenuSceneStarter
    {
        void StartMenuScene(User user, ILoadingScreen loadingScreen);
        void ConfirmStartingMenuScene(User user, ILoadingScreen loadingScreen);
    }
}

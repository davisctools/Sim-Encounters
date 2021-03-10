using ClinicalTools.UI;

namespace ClinicalTools.SimEncounters
{
    public class UserMenuSceneStarter : IUserMenuSceneStarter
    {
        protected IMenuSceneStarter MenuSceneStarter { get; set; }
        protected IMenuEncountersInfoReader MenuInfoReader { get; set; }
        protected BaseConfirmationPopup ConfirmationPopup { get; set; }

        public UserMenuSceneStarter(
            IMenuSceneStarter menuSceneStarter,
            IMenuEncountersInfoReader menuInfoReader,
            BaseConfirmationPopup confirmationPopup)
        {
            MenuSceneStarter = menuSceneStarter;
            MenuInfoReader = menuInfoReader;
            ConfirmationPopup = confirmationPopup;
        }

        public virtual void StartMenuScene(User user, ILoadingScreen loadingScreen, MenuArea menuArea)
        {
            var categories = MenuInfoReader.GetMenuEncountersInfo(user);
            var menuSceneInfo = new LoadingMenuSceneInfo(user, loadingScreen, menuArea, categories);
            MenuSceneStarter.StartScene(menuSceneInfo);
        }

        protected User User { get; set; }
        protected ILoadingScreen LoadingScreen { get; set; }
        protected MenuArea MenuArea { get; set; }

        protected virtual string ExitConfirmationTitle { get; } = "RETURN TO MAIN MENU";
        protected virtual string ExitConfirmationDescription { get; } = "Are you sure you want to exit?";
        public virtual void ConfirmStartingMenuScene(User user, ILoadingScreen loadingScreen, MenuArea menuArea)
        {
            User = user;
            LoadingScreen = loadingScreen;
            MenuArea = menuArea;
            ConfirmationPopup.ShowConfirmation(ExitScene, ExitConfirmationTitle,
                ExitConfirmationDescription);
        }
        protected virtual void ExitScene() => StartMenuScene(User, LoadingScreen, MenuArea);

    }
}

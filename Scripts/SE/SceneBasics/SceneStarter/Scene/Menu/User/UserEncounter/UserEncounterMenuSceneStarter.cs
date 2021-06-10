using ClinicalTools.UI;

namespace ClinicalTools.SimEncounters
{
    public class UserEncounterMenuSceneStarter : UserMenuSceneStarter, IUserEncounterMenuSceneStarter
    {
        public UserEncounterMenuSceneStarter(
               IMenuSceneStarter menuSceneStarter,
               IMenuEncountersInfoReader menuInfoReader,
               BaseConfirmationPopup confirmationPopup)
            : base(menuSceneStarter, menuInfoReader, confirmationPopup) { }

        protected override string ExitConfirmationTitle { get; } = "Exit to Main Menu";

        public void StartMenuScene(UserEncounter userEncounter, ILoadingScreen loadingScreen, MenuArea menuArea)
        {
            ImageHolder.BeginHoldingData();
            var categories = GetMenuEncountersInfo(userEncounter);
            var menuSceneInfo = new LoadingMenuSceneInfo(userEncounter.User, loadingScreen, menuArea, categories);
            MenuSceneStarter.StartScene(menuSceneInfo);
        }

        protected UserEncounter UserEncounter { get; set; }

        public void ConfirmStartingMenuScene(UserEncounter userEncounter, ILoadingScreen loadingScreen, MenuArea menuArea)
        {
            UserEncounter = userEncounter;
            LoadingScreen = loadingScreen;
            MenuArea = menuArea;
            ConfirmationPopup.ShowConfirmation(ExitSceneWithEncounter, ExitConfirmationTitle,
                ExitConfirmationDescription, "EXIT", "CANCEL");
        }

        protected virtual void ExitSceneWithEncounter() => StartMenuScene(UserEncounter, LoadingScreen, MenuArea);

        protected virtual WaitableTask<IMenuEncountersInfo> GetMenuEncountersInfo(
            UserEncounter userEncounter)
        {
            var task = new WaitableTask<IMenuEncountersInfo>();
            var categories = MenuInfoReader.GetMenuEncountersInfo(userEncounter.User);
            categories.AddOnCompletedListener((result) =>
                CompleteMenuEncountersInfoTask(task, result, userEncounter));
            return task;
        }

        protected virtual void CompleteMenuEncountersInfoTask(WaitableTask<IMenuEncountersInfo> task,
            TaskResult<IMenuEncountersInfo> result, UserEncounter userEncounter)
        {
            if (userEncounter?.Status != null)
                userEncounter.Status.BasicStatus.Completed = userEncounter.Status.ContentStatus.Read;

            if (result.IsError()) {
                task.SetError(result.Exception);
                return;
            }

            if (result.Value == null) {
                task.SetResult(null);
                return;
            }

            foreach (var encounter in result.Value.GetEncounters()) {
                if (encounter.GetLatestMetadata().RecordNumber != userEncounter.Data.Metadata.RecordNumber)
                    continue;

                if (userEncounter.Status != null)
                    encounter.Status = userEncounter.Status.BasicStatus;
                else
                    encounter.Status = new EncounterBasicStatus();
                break;
            }

            task.SetResult(result.Value);
        }
    }
}

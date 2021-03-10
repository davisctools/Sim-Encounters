namespace ClinicalTools.SimEncounters
{
    public interface IUserEncounterMenuSceneStarter : IUserMenuSceneStarter
    {
        void StartMenuScene(UserEncounter userEncounter, ILoadingScreen loadingScreen, MenuArea menuArea);
        void ConfirmStartingMenuScene(UserEncounter userEncounter, ILoadingScreen loadingScreen, MenuArea menuArea);
    }
}

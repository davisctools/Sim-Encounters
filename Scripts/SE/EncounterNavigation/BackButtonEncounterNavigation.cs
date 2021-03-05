using ClinicalTools.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class BackButtonEncounterNavigation
    {
        protected ILinearEncounterNavigator LinearEncounterNavigator { get; set; }
        protected IUserEncounterMenuSceneStarter MenuSceneStarter { get; set; }
        protected AndroidBackButton BackButton { get; set; }
        [Inject]
        public virtual void Inject(
            ILinearEncounterNavigator linearEncounterNavigator,
            IUserEncounterMenuSceneStarter menuSceneStarter,
            AndroidBackButton backButton)
        {
            LinearEncounterNavigator = linearEncounterNavigator;            
            MenuSceneStarter = menuSceneStarter;
            
            BackButton = backButton;
            BackButton.Register(BackButtonThing);
        }

        protected virtual void BackButtonThing()
        {
            BackButton.Register(BackButtonThing);
            if (LinearEncounterNavigator.HasPrevious())
                LinearEncounterNavigator.GoToPrevious();

        }
    }
}
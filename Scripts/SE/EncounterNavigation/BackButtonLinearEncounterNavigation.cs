using ClinicalTools.UI;
using System.Collections;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class BackButtonLinearEncounterNavigation
    {
        protected ILinearEncounterNavigator LinearEncounterNavigator { get; set; }
        protected AndroidBackButton BackButton { get; set; }
        [Inject]
        public virtual void Inject(
            ILinearEncounterNavigator linearEncounterNavigator,
            AndroidBackButton backButton)
        {
            LinearEncounterNavigator = linearEncounterNavigator;

            BackButton = backButton;
            BackButton.Register(OnBackButton);
        }

        protected virtual void OnBackButton()
        {
            BackButton.Register(OnBackButton);
            if (LinearEncounterNavigator.HasPrevious())
                LinearEncounterNavigator.GoToPrevious();

        }
    }
}
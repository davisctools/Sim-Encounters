using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ReaderMobileFooter : MonoBehaviour
    {
        public virtual Button NextButton { get => nextButton; set => nextButton = value; }
        [SerializeField] private Button nextButton;
        public virtual Button PreviousButton { get => previousButton; set => previousButton = value; }
        [SerializeField] private Button previousButton;
        public virtual Button PrimaryFinishButton { get => primaryFinishButton; set => primaryFinishButton = value; }
        [SerializeField] private Button primaryFinishButton;

        protected ILinearEncounterNavigator LinearEncounterNavigator { get; set; }
        protected ISelector<UserEncounterSelectedEventArgs> UserEncounterSelector { get; set; }
        protected ISelector<UserTabSelectedEventArgs> UserTabSelector { get; set; }
        protected SignalBus SignalBus { get; set; }
        protected IUserEncounterMenuSceneStarter MenuSceneStarter { get; set; }
        [Inject]
        public virtual void Inject(
            ILinearEncounterNavigator linearEncounterNavigator,
            ISelector<UserEncounterSelectedEventArgs> userEncounterSelector,
            ISelector<UserTabSelectedEventArgs> userTabSelector,
            SignalBus signalBus,
            IUserEncounterMenuSceneStarter menuSceneStarter)
        {
            UserEncounterSelector = userEncounterSelector;
            UserTabSelector = userTabSelector;

            LinearEncounterNavigator = linearEncounterNavigator;
            SignalBus = signalBus;
            MenuSceneStarter = menuSceneStarter;
        }

        protected virtual void Awake()
        {
            NextButton.onClick.AddListener(GoToNext);
            if (PreviousButton != null)
                PreviousButton.onClick.AddListener(GoToPrevious);
            PrimaryFinishButton.onClick.AddListener(Complete);
        }

        protected virtual void Start()
        {
            UserEncounterSelector.Selected += OnEncounterSelected;
            if (UserEncounterSelector.CurrentValue != null)
                OnEncounterSelected(UserEncounterSelector, UserEncounterSelector.CurrentValue);

            UserTabSelector.Selected += OnTabSelected;
            if (UserTabSelector.CurrentValue != null)
                OnTabSelected(UserTabSelector, UserTabSelector.CurrentValue);
        }

        protected UserEncounter UserEncounter { get; set; }
        protected EncounterContent Content => UserEncounter.Data.Content;
        protected virtual void OnEncounterSelected(object sender, UserEncounterSelectedEventArgs eventArgs)
        {
            UserEncounter = eventArgs.Encounter;

            if (UserEncounter.IsRead())
                EnableFinishButton();
            else
                UserEncounter.StatusChanged += UpdateFinishButtonActive;
        }

        protected virtual void UpdateFinishButtonActive()
        {
            if (!UserEncounter.IsRead())
                return;
            UserEncounter.StatusChanged -= UpdateFinishButtonActive;
            EnableFinishButton();
        }

        protected virtual void EnableFinishButton()
            => PrimaryFinishButton.gameObject.SetActive(true);

        protected UserTab CurrentTab { get; set; }
        protected virtual void OnTabSelected(object sender, UserTabSelectedEventArgs eventArgs)
        {
            if (CurrentTab == eventArgs.SelectedTab)
                return;
            CurrentTab = eventArgs.SelectedTab;

            if (PreviousButton != null)
                PreviousButton.gameObject.SetActive(LinearEncounterNavigator.HasPrevious());

            NextButton.gameObject.SetActive(LinearEncounterNavigator.HasNext());
        }

        protected virtual void GoToNext() => LinearEncounterNavigator.GoToNext();
        protected virtual void GoToPrevious() => LinearEncounterNavigator.GoToPrevious();
        protected virtual void Complete() => SignalBus.Fire<EncounterCompletedSignal>();
    }
}
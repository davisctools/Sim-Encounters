using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ShowEncounterInfoButton : MonoBehaviour
    {
        public Button Button { get => button; set => button = value; }
        [SerializeField] private Button button;

        protected UserEncounter CurrentUserEncounter { get; set; }
        protected BaseReaderEncounterInfoPopup EncounterInfoPopup { get; set; }
        protected ISelector<UserEncounterSelectedEventArgs> EncounterSelector { get; set; }
        protected ISelectedListener<UserEncounterSelectedEventArgs> EncounterSelectedListener { get; set; }
        [Inject]
        public virtual void Inject(
            ISelector<UserEncounterSelectedEventArgs> encounterSelector,
            ISelectedListener<UserEncounterSelectedEventArgs> encounterSelectedListener,
            BaseReaderEncounterInfoPopup encounterInfoPopup)
        {
            EncounterSelector = encounterSelector;
            EncounterSelectedListener = encounterSelectedListener;
            EncounterInfoPopup = encounterInfoPopup;
        }
        protected virtual void Start()
        {
            EncounterSelectedListener.Selected += OnUserEncounterSelected;
            if (EncounterSelectedListener.CurrentValue != null)
                OnUserEncounterSelected(EncounterSelector, EncounterSelectedListener.CurrentValue);
        }
        protected virtual void OnUserEncounterSelected(object sender, UserEncounterSelectedEventArgs eventArgs) 
            => CurrentUserEncounter = eventArgs.Encounter;

        protected virtual void OnDestroy() => EncounterSelectedListener.Selected -= OnUserEncounterSelected;
        protected virtual void Awake() => Button.onClick.AddListener(ShowEncounterInfo);
        public virtual void ShowEncounterInfo() => EncounterInfoPopup.ShowEncounterInfo(CurrentUserEncounter);
    }
}
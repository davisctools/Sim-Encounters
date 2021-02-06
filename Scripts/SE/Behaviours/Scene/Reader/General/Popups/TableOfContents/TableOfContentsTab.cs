using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using ClinicalTools.SEColors;

namespace ClinicalTools.SimEncounters
{
    public class TableOfContentsTab : BaseSelectableUserTabBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private TMP_Text text;
        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI label;

        protected ISelectedListener<UserEncounterSelectedEventArgs> EncounterSelectedListener { get; set; }
        protected ISelector<UserSectionSelectedEventArgs> SectionSelector { get; set; }
        [Inject]
        public virtual void Inject(ISelectedListener<UserEncounterSelectedEventArgs> encounterSelectedListener,
            ISelector<UserSectionSelectedEventArgs> sectionSelector)
        {
            EncounterSelectedListener = encounterSelectedListener;
            SectionSelector = sectionSelector;
        }

        protected virtual void Start() => button.onClick.AddListener(SelectTab);
        protected virtual void OnEnable() => UpdateColors();

        public override void Initialize(UserTab tab)
        {
            base.Initialize(tab);
            UpdateColors();
        }

        protected virtual void SelectTab()
        {
            var Section = SectionSelector.CurrentValue.SelectedSection;
            Section.Data.SetCurrentTab(Tab.Data);
            SectionSelector.Select(this, new UserSectionSelectedEventArgs(Section, ChangeType.JumpTo));
            TabSelector.Select(this, new UserTabSelectedEventArgs(Tab, ChangeType.JumpTo));
        }

        protected virtual void UpdateColors()
        {
            if (Tab == null)
                return;

            var colorManager = new ColorManager();
            var isCurrentTab = EncounterSelectedListener.CurrentValue.Encounter.GetCurrentSection().GetCurrentTab() == Tab;
            var sectionColor = SectionSelector.CurrentValue.SelectedSection.Data.Color;
            image.color = isCurrentTab ? sectionColor : colorManager.GetColor(ColorType.Gray1);
            text.color = isCurrentTab ? Color.white : sectionColor;
        }
    }
}
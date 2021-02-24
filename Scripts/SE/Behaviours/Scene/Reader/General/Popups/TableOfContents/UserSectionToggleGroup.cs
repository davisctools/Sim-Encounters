using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{

    public class UserSectionToggleGroup : BaseUserSectionSelectorGroup<UserSectionToggle>
    {
        [SerializeField] private ToggleGroup toggleGroup;

        protected ISelectedListener<UserSectionSelectedEventArgs> SectionSelector { get; set; }
        [Inject]
        public virtual void Inject(ISelectedListener<UserSectionSelectedEventArgs> sectionSelector)
            => SectionSelector = sectionSelector;

        protected virtual void Awake() => SectionSelector.Selected += OnSectionSelected;

        protected override void OnEncounterSelected(object sender, UserEncounterSelectedEventArgs e)
        {
            base.OnEncounterSelected(sender, e);
            SectionButtons[e.Encounter.GetCurrentSection()].Select();
        }

        protected virtual void OnSectionSelected(object sender, UserSectionSelectedEventArgs e)
            => SectionButtons[e.SelectedSection].Select();

        protected override UserSectionToggle CreateSectionObject()
        {
            var sectionObject = base.CreateSectionObject();
            sectionObject.SetToggleGroup(toggleGroup);
            return sectionObject;
        }
    }
}
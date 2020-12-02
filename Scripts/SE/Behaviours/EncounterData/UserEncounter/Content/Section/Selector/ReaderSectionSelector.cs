using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ReaderSectionSelector : MonoBehaviour
    {
        public virtual bool StartSectionAtFirstTab { get => startSectionAtFirstTab; set => startSectionAtFirstTab = value; }
        [SerializeField] private bool startSectionAtFirstTab;

        public virtual Transform SectionButtonsParent { get => sectionButtonsParent; set => sectionButtonsParent = value; }
        [SerializeField] private Transform sectionButtonsParent;
        public virtual ToggleGroup SectionsToggleGroup { get => sectionsToggleGroup; set => sectionsToggleGroup = value; }
        [SerializeField] private ToggleGroup sectionsToggleGroup;

        protected ReaderSectionToggle.Factory SectionButtonFactory { get; set; }
        protected ISelector<UserEncounterSelectedEventArgs> UserEncounterSelector { get; set; }
        protected ISelector<UserSectionSelectedEventArgs> UserSectionSelector { get; set; }
        [Inject]
        public virtual void Inject(
            ReaderSectionToggle.Factory sectionButtonFactory,
            ISelector<UserEncounterSelectedEventArgs> userEncounterSelector,
            ISelector<UserSectionSelectedEventArgs> userSectionSelector)
        {
            SectionButtonFactory = sectionButtonFactory;
            UserEncounterSelector = userEncounterSelector;
            UserSectionSelector = userSectionSelector;
        }
        protected virtual void Start()
        {
            UserEncounterSelector.Selected += OnEncounterSelected;
            if (UserEncounterSelector.CurrentValue != null)
                OnEncounterSelected(UserEncounterSelector, UserEncounterSelector.CurrentValue);

            UserSectionSelector.Selected += OnSectionSelected;
            if (UserSectionSelector.CurrentValue != null)
                OnSectionSelected(UserSectionSelector, UserSectionSelector.CurrentValue);
        }

        protected virtual void OnDestroy()
        {
            UserEncounterSelector.Selected -= OnEncounterSelected;
            UserSectionSelector.Selected -= OnSectionSelected;
        }

        protected UserEncounter UserEncounter { get; set; }
        protected Dictionary<UserSection, ReaderSectionToggle> SectionButtons { get; } 
            = new Dictionary<UserSection, ReaderSectionToggle>();
        protected virtual void OnEncounterSelected(object sender, UserEncounterSelectedEventArgs eventArgs)
        {
            foreach (var sectionButton in SectionButtons)
                Destroy(sectionButton.Value.gameObject);
            SectionButtons.Clear();

            UserEncounter = eventArgs.Encounter;
            foreach (var userSection in UserEncounter.Sections)
                AddButton(userSection.Value);
        }

        protected void AddButton(UserSection section)
        {
            var sectionButton = SectionButtonFactory.Create();
            sectionButton.transform.SetParent(SectionButtonsParent);
            sectionButton.SetToggleGroup(SectionsToggleGroup);
            sectionButton.Display(section);
            sectionButton.Selected += () => OnSelected(section);
            SectionButtons.Add(section, sectionButton);
        }

        protected UserSection CurrentSection { get; set; }
        protected void OnSelected(UserSection section)
        {
            if (CurrentSection == section)
                return;

            if (StartSectionAtFirstTab)
                section.Data.CurrentTabIndex = 0;
            CurrentSection = section;

            var selectedArgs = new UserSectionSelectedEventArgs(section, ChangeType.JumpTo);
            UserSectionSelector?.Select(this, selectedArgs);
        }


        protected virtual void OnSectionSelected(object sender, UserSectionSelectedEventArgs eventArgs)
        {
            if (CurrentSection == eventArgs.SelectedSection)
                return;

            CurrentSection = eventArgs.SelectedSection;
            SectionButtons[CurrentSection].Select();
            foreach (var sectionButton in SectionButtons) {
                if (sectionButton.Key == CurrentSection)
                    continue;
                sectionButton.Value.Deselect();
            }
        }
    }
}
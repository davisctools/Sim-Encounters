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
        protected ISelectedListener<UserEncounterSelectedEventArgs> EncounterSelectedListener { get; set; }
        protected ISelector<UserSectionSelectedEventArgs> SectionSelector { get; set; }
        protected ISelectedListener<UserSectionSelectedEventArgs> SectionSelectedListener { get; set; }
        [Inject]
        public virtual void Inject(
            ReaderSectionToggle.Factory sectionButtonFactory,
            ISelectedListener<UserEncounterSelectedEventArgs> encounterSelectedListener,
            ISelector<UserSectionSelectedEventArgs> sectionSelector,
            ISelectedListener<UserSectionSelectedEventArgs> sectionSelectedListener)
        {
            SectionButtonFactory = sectionButtonFactory;
            EncounterSelectedListener = encounterSelectedListener;
            SectionSelector = sectionSelector;
            SectionSelectedListener = sectionSelectedListener;
        }
        protected virtual void Start()
        {
            EncounterSelectedListener.Selected += OnEncounterSelected;
            if (EncounterSelectedListener.CurrentValue != null)
                OnEncounterSelected(EncounterSelectedListener, EncounterSelectedListener.CurrentValue);

            SectionSelectedListener.Selected += OnSectionSelected;
            if (SectionSelectedListener.CurrentValue != null)
                OnSectionSelected(SectionSelector, SectionSelectedListener.CurrentValue);
        }

        protected virtual void OnDestroy()
        {
            EncounterSelectedListener.Selected -= OnEncounterSelected;
            SectionSelectedListener.Selected -= OnSectionSelected;
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
            SectionSelector?.Select(this, selectedArgs);
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
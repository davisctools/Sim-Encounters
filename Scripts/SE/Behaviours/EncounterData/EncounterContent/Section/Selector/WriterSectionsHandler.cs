using ClinicalTools.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class WriterSectionsHandler : MonoBehaviour
    {
        public virtual BaseRearrangeableGroup RearrangeableGroup { get => rearrangeableGroup; set => rearrangeableGroup = value; }
        [SerializeField] private BaseRearrangeableGroup rearrangeableGroup;
        public virtual BaseWriterSectionToggle SectionButtonPrefab { get => sectionButtonPrefab; set => sectionButtonPrefab = value; }
        [SerializeField] private BaseWriterSectionToggle sectionButtonPrefab;
        public virtual ToggleGroup SectionsToggleGroup { get => sectionsToggleGroup; set => sectionsToggleGroup = value; }
        [SerializeField] private ToggleGroup sectionsToggleGroup;
        public virtual Button AddButton { get => addButton; set => addButton = value; }
        [SerializeField] private Button addButton;
        public SectionCreatorPopup AddSectionPopup { get => addSectionPopup; set => addSectionPopup = value; }
        [SerializeField] private SectionCreatorPopup addSectionPopup;

        protected ISelectedListener<EncounterSelectedEventArgs> EncounterSelectedListener { get; set; }
        protected ISelector<SectionSelectedEventArgs> SectionSelector { get; set; }
        protected virtual BaseWriterSectionToggle.Pool SectionButtonPool { get; set; }
        [Inject]
        public virtual void Inject(
            ISelectedListener<EncounterSelectedEventArgs> encounterSelectedListener,
            ISelector<SectionSelectedEventArgs> sectionSelectedListener,
            BaseWriterSectionToggle.Pool sectionButtonPool)
        {
            SectionButtonPool = sectionButtonPool;

            EncounterSelectedListener = encounterSelectedListener;
            EncounterSelectedListener.Selected += OnEncounterSelected;
            if (EncounterSelectedListener.CurrentValue != null)
                OnEncounterSelected(this, EncounterSelectedListener.CurrentValue);

            SectionSelector = sectionSelectedListener;
            SectionSelector.Selected += OnSectionSelected;
            if (SectionSelector.CurrentValue != null)
                OnSectionSelected(this, SectionSelector.CurrentValue);
        }

        protected Encounter CurrentEncounter { get; set; }
        protected Dictionary<Section, BaseWriterSectionToggle> SectionButtons { get; } = new Dictionary<Section, BaseWriterSectionToggle>();

        protected virtual void Awake()
        {
            AddButton.onClick.AddListener(AddSection);
            RearrangeableGroup.Rearranged += SectionsRearranged;
        }

        protected virtual void OnEncounterSelected(object sender, EncounterSelectedEventArgs e)
        {
            RearrangeableGroup.Clear();
            foreach (var sectionButton in SectionButtons)
                Destroy(sectionButton.Value.gameObject);
            SectionButtons.Clear();

            CurrentEncounter = e.Encounter;
            foreach (var section in CurrentEncounter.Content.NonImageContent.Sections)
                AddSectionButton(CurrentEncounter, section.Value);
        }


        private void OnSectionSelected(object sender, SectionSelectedEventArgs e)
        {
            if (CurrentSection == e.SelectedSection)
                return;

            CurrentSection = e.SelectedSection;
            SectionButtons[CurrentSection].Select();
        }

        protected virtual void AddSection()
        {
            var newSection = AddSectionPopup.CreateSection(CurrentEncounter);
            newSection.AddOnCompletedListener(AddNewSection);
        }

        protected virtual void AddNewSection(TaskResult<Section> section)
        {
            if (section.IsError() || section.Value == null)
                return;

            CurrentEncounter.Content.NonImageContent.Sections.Add(section.Value);
            AddSectionButton(CurrentEncounter, section.Value);

            var sectionSelectedArgs = new SectionSelectedEventArgs(section.Value);
            SectionSelector.Select(this, sectionSelectedArgs);
        }

        protected void AddSectionButton(Encounter encounter, Section section)
        {
            var sectionButton = SectionButtonPool.Spawn();
            RearrangeableGroup.Add(sectionButton);
            sectionButton.RectTransform.localScale = Vector3.one;
            sectionButton.SetToggleGroup(SectionsToggleGroup);
            sectionButton.Display(encounter, section);
            sectionButton.Selected += () => OnSelected(section);
            sectionButton.Edited += OnSectionEdited;
            sectionButton.Deleted += OnDeleted;
            SectionButtons.Add(section, sectionButton);
        }

        protected virtual void OnSectionEdited(Section section)
        {
            if (section == SectionSelector.CurrentValue.SelectedSection)
                SectionSelector.Select(this, new SectionSelectedEventArgs(section, SelectionType.Edited));
        }

        protected Section CurrentSection { get; set; }
        protected void OnSelected(Section section)
        {
            if (CurrentSection == section)
                return;

            var selectedArgs = new SectionSelectedEventArgs(section);
            CurrentSection = section;
            SectionSelector.Select(this, selectedArgs);
        }
        protected void OnEdited(Section section)
        {
            var selectedArgs = new SectionSelectedEventArgs(section);
            CurrentSection = section;
            SectionSelector.Select(this, selectedArgs);
        }
        protected void OnDeleted(Section section)
        {
            var button = SectionButtons[section];
            RearrangeableGroup.Remove(button);
            SectionButtons.Remove(section);
            CurrentEncounter.Content.NonImageContent.Sections.Remove(section);

            CurrentSection = section;
            SectionButtonPool.Despawn(button);
        }

        private void SectionsRearranged(object sender, RearrangedEventArgs2 e)
        {
            var sections = CurrentEncounter.Content.NonImageContent.Sections;
            sections.MoveValue(e.NewIndex, e.OldIndex);
        }
    }
}
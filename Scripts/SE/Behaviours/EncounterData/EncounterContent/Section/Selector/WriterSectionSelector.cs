﻿using ClinicalTools.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class WriterSectionSelector : MonoBehaviour
    {
        public virtual BaseRearrangeableGroup RearrangeableGroup { get => rearrangeableGroup; set => rearrangeableGroup = value; }
        [SerializeField] private BaseRearrangeableGroup rearrangeableGroup;
        public virtual BaseWriterSectionToggle SectionButtonPrefab { get => sectionButtonPrefab; set => sectionButtonPrefab = value; }
        [SerializeField] private BaseWriterSectionToggle sectionButtonPrefab;
        public virtual ToggleGroup SectionsToggleGroup { get => sectionsToggleGroup; set => sectionsToggleGroup = value; }
        [SerializeField] private ToggleGroup sectionsToggleGroup;
        public virtual Button AddButton { get => addButton; set => addButton = value; }
        [SerializeField] private Button addButton;

        protected ISelectedListener<EncounterSelectedEventArgs> EncounterSelectedListener { get; set; }
        protected ISelector<SectionSelectedEventArgs> SectionSelector { get; set; }
        protected ISelectedListener<SectionSelectedEventArgs> SectionSelectedListener { get; set; }
        protected virtual BaseWriterSectionToggle.Pool SectionButtonPool { get; set; }
        protected virtual SectionCreatorPopup AddSectionPopup { get; set; }

        [Inject]
        public virtual void Inject(
            ISelectedListener<EncounterSelectedEventArgs> encounterSelectedListener,
            ISelector<SectionSelectedEventArgs> sectionSelector,
            ISelectedListener<SectionSelectedEventArgs> sectionSelectedListener,
            BaseWriterSectionToggle.Pool sectionButtonPool,
            SectionCreatorPopup addSectionPopup)
        {
            SectionButtonPool = sectionButtonPool;
            AddSectionPopup = addSectionPopup;

            EncounterSelectedListener = encounterSelectedListener;
            EncounterSelectedListener.Selected += OnEncounterSelected;
            if (EncounterSelectedListener.CurrentValue != null)
                OnEncounterSelected(this, EncounterSelectedListener.CurrentValue);

            SectionSelector = sectionSelector;

            SectionSelectedListener = sectionSelectedListener;
            SectionSelectedListener.Selected += OnSectionSelected;
            if (SectionSelectedListener.CurrentValue != null)
                OnSectionSelected(this, SectionSelectedListener.CurrentValue);
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
            foreach (var sectionButton in SectionButtons)
                DespawnButton(sectionButton.Value);
            RearrangeableGroup.Clear();
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

            sectionButton.Selected += OnSelected;
            sectionButton.Edited += OnSectionEdited;
            sectionButton.Deleted += OnDeleted;

            SectionButtons.Add(section, sectionButton);
        }

        protected virtual void OnSectionEdited(Section section)
        {
            if (section == SectionSelectedListener.CurrentValue.SelectedSection)
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
            var nonImageContent = CurrentEncounter.Content.NonImageContent;
            var sections = nonImageContent.Sections;
            var sectionIndex = nonImageContent.CurrentSectionIndex;
            var button = SectionButtons[section];

            RearrangeableGroup.Remove(button);
            SectionButtons.Remove(section);
            sections.Remove(section);
            DespawnButton(button);

            if (sections.Count == 0)
                return;

            if (sectionIndex == sections.Count)
                sectionIndex--;
            SectionSelector.Select(this, new SectionSelectedEventArgs(sections[sectionIndex].Value));
        }

        protected virtual void DespawnButton(BaseWriterSectionToggle button)
        {
            button.Selected -= OnSelected;
            button.Edited -= OnSectionEdited;
            button.Deleted -= OnDeleted;
            SectionButtonPool.Despawn(button);
        }

        protected virtual void SectionsRearranged(object sender, RearrangedEventArgs2 e)
            => CurrentEncounter.Content.NonImageContent.Sections.MoveValue(e.NewIndex, e.OldIndex);
    }
}
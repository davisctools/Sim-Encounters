﻿using TMPro;
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
        [SerializeField] private bool highlightIfCurrent = true;
        [SerializeField] private bool setTextColor = true;

        protected virtual IColorManager ColorManager { get; } = new ColorManager();

        protected ISelectedListener<UserEncounterSelectedEventArgs> EncounterSelectedListener { get; set; }
        protected ISelector<UserSectionSelectedEventArgs> SectionSelector { get; set; }
        protected ISelectedListener<UserSectionSelectedEventArgs> SectionSelectedListener { get; set; }
        [Inject]
        public virtual void Inject(
            ISelectedListener<UserEncounterSelectedEventArgs> encounterSelectedListener,
            ISelector<UserSectionSelectedEventArgs> sectionSelector,
            ISelectedListener<UserSectionSelectedEventArgs> sectionSelectedListener)
        {
            EncounterSelectedListener = encounterSelectedListener;
            SectionSelector = sectionSelector;
            SectionSelectedListener = sectionSelectedListener;
        }

        protected virtual void Start() => button.onClick.AddListener(SelectTab);
        protected virtual void OnEnable() => Refresh();

        public override void Initialize(UserTab tab)
        {
            base.Initialize(tab);
            Refresh();
        }

        protected virtual void SelectTab()
        {
            var Section = SectionSelectedListener.CurrentValue.SelectedSection;
            Section.Data.SetCurrentTab(Tab.Data);
            SectionSelector.Select(this, new UserSectionSelectedEventArgs(Section, ChangeType.JumpTo));
            TabSelector.Select(this, new UserTabSelectedEventArgs(Tab, ChangeType.JumpTo));
            Refresh();
        }

        protected virtual void LateUpdate()
        {
            if (!isCurrentTab)
                return;

            isCurrentTab = EncounterSelectedListener.CurrentValue.Encounter.GetCurrentSection().GetCurrentTab() == Tab;
            if (!isCurrentTab)
                UpdateColors(isCurrentTab);
        }

        private bool isCurrentTab;
        protected virtual void Refresh()
        {
            if (Tab == null)
                return;

            isCurrentTab = EncounterSelectedListener.CurrentValue.Encounter.GetCurrentSection().GetCurrentTab() == Tab;
            UpdateColors(isCurrentTab);
        }

        protected virtual void UpdateColors(bool isCurrentTab)
        {
            var sectionColor = SectionSelectedListener.CurrentValue.SelectedSection.Data.Color;
            image.color = highlightIfCurrent && isCurrentTab ? sectionColor : Color.white;
            if (setTextColor)
                text.color = highlightIfCurrent && isCurrentTab ? Color.white : ColorManager.GetColor(ColorType.Gray6);
        }
    }
}
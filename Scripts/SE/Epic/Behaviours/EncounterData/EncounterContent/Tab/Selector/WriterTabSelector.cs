using ClinicalTools.UI;
using ClinicalTools.UI.Extensions;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class WriterTabSelector : MonoBehaviour
    {
        public virtual BaseRearrangeableGroup RearrangeableGroup { get => rearrangeableGroup; set => rearrangeableGroup = value; }
        [SerializeField] private BaseRearrangeableGroup rearrangeableGroup;
        public virtual BaseWriterTabToggle TabButtonPrefab { get => tabButtonPrefab; set => tabButtonPrefab = value; }
        [SerializeField] private BaseWriterTabToggle tabButtonPrefab;
        public virtual ToggleGroup TabsToggleGroup { get => tabsToggleGroup; set => tabsToggleGroup = value; }
        [SerializeField] private ToggleGroup tabsToggleGroup;
        public virtual ScrollRect TabButtonsScroll { get => tabButtonsScroll; set => tabButtonsScroll = value; }
        [SerializeField] private ScrollRect tabButtonsScroll;
        public virtual Button AddButton { get => addButton; set => addButton = value; }
        [SerializeField] private Button addButton;
        public TabCreatorPopup AddTabPopup { get => addTabPopup; set => addTabPopup = value; }
        [SerializeField] private TabCreatorPopup addTabPopup;

        protected ISelectedListener<SectionSelectedEventArgs> SectionSelectedListener { get; set; }
        protected ISelector<TabSelectedEventArgs> TabSelector { get; set; }
        protected virtual BaseWriterTabToggle.Pool TabButtonPool { get; set; }
        [Inject]
        public virtual void Inject(
            ISelectedListener<SectionSelectedEventArgs> sectionSelectedListener,
            ISelector<TabSelectedEventArgs> tabSelector,
            BaseWriterTabToggle.Pool tabButtonPool)
        {
            TabButtonPool = tabButtonPool;

            SectionSelectedListener = sectionSelectedListener;
            SectionSelectedListener.Selected += OnSectionSelected;
            if (SectionSelectedListener.CurrentValue != null)
                OnSectionSelected(this, SectionSelectedListener.CurrentValue);

            TabSelector = tabSelector;
            TabSelector.Selected += OnTabSelected;
            if (TabSelector.CurrentValue != null)
                OnTabSelected(this, TabSelector.CurrentValue);

        }

        protected virtual void Awake()
        {
            AddButton.onClick.AddListener(AddTab);
            RearrangeableGroup.Rearranged += TabsRearranged;
        }

        protected virtual void OnSectionSelected(object sender, SectionSelectedEventArgs e)
        {
            CurrentSection = e.SelectedSection;
            RearrangeableGroup.Clear();
            foreach (var tabButton in TabButtons)
                Destroy(tabButton.Value.gameObject);

            TabButtons.Clear();

            if (CurrentSection.Tabs.Count == 0) {
                AddTab();
            } else {
                foreach (var tab in CurrentSection.Tabs)
                    AddTabButton(CurrentSection.Tabs[tab.Key]);
            }
        }

        protected virtual void OnTabSelected(object sender, TabSelectedEventArgs e)
        {
            if (e.SelectedTab == CurrentTab)
                return;

            CurrentTab = e.SelectedTab;
            TabButtons[CurrentTab].Select();
        }

        private void AddTab()
        {
            var newTab = AddTabPopup.CreateTab();
            newTab.AddOnCompletedListener(AddNewTab);
        }

        private void AddNewTab(TaskResult<Tab> tab)
        {
            if (tab.IsError() || tab.Value == null)
                return;

            CurrentSection.Tabs.Add(tab.Value);
            AddTabButton(tab.Value);

            TabSelector.Select(this, new TabSelectedEventArgs(tab.Value));
        }

        protected Section CurrentSection { get; set; }

        protected Dictionary<Tab, BaseWriterTabToggle> TabButtons { get; } = new Dictionary<Tab, BaseWriterTabToggle>();
        protected void AddTabButton(Tab tab)
        {
            var tabButton = TabButtonPool.Spawn();
            RearrangeableGroup.Add(tabButton);
            tabButton.RectTransform.localScale = Vector3.one;
            tabButton.SetToggleGroup(TabsToggleGroup);
            tabButton.Display(tab);
            tabButton.Selected += () => OnSelected(tab);
            tabButton.Deleted += OnDeleted;
            TabButtons.Add(tab, tabButton);
        }

        protected Tab CurrentTab { get; set; }
        protected void OnSelected(Tab tab)
        {
            var selectedArgs = new TabSelectedEventArgs(tab);
            CurrentTab = tab;
            TabSelector.Select(this, selectedArgs);

            TabButtonsScroll.EnsureChildIsShowing(TabButtons[tab].RectTransform);
        }
        protected void OnDeleted(Tab tab)
        {
            var tabButton = TabButtons[tab];
            rearrangeableGroup.Remove(tabButton);
            TabButtons.Remove(tab);
            CurrentSection.Tabs.Remove(tab);
            TabButtonPool.Despawn(tabButton);
        }

        protected virtual void TabsRearranged(object sender, RearrangedEventArgs2 e)
            => CurrentSection.Tabs.MoveValue(e.NewIndex, e.OldIndex);
    }
}
using ClinicalTools.UI;
using ClinicalTools.UI.Extensions;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ReaderTabSelector : MonoBehaviour
    {
        public virtual Transform TabButtonsParent { get => tabButtonsParent; set => tabButtonsParent = value; }
        [SerializeField] private Transform tabButtonsParent;
        public virtual BaseReaderTabToggle TabButtonPrefab { get => tabButtonPrefab; set => tabButtonPrefab = value; }
        [SerializeField] private BaseReaderTabToggle tabButtonPrefab;
        public virtual ToggleGroup TabsToggleGroup { get => tabsToggleGroup; set => tabsToggleGroup = value; }
        [SerializeField] private ToggleGroup tabsToggleGroup;
        public virtual ScrollRect TabButtonsScroll { get => tabButtonsScroll; set => tabButtonsScroll = value; }
        [SerializeField] private ScrollRect tabButtonsScroll;

        protected BaseReaderTabToggle.Pool TabButtonPool { get; set; }
        protected ISelector<UserSectionSelectedEventArgs> UserSectionSelector { get; set; }
        protected ISelector<UserTabSelectedEventArgs> UserTabSelector { get; set; }
        [Inject]
        public virtual void Inject(
            BaseReaderTabToggle.Pool tabButtonPool,
            ISelector<UserSectionSelectedEventArgs> userSectionSelector,
            ISelector<UserTabSelectedEventArgs> userTabSelector)
        {
            TabButtonPool = tabButtonPool;
            UserSectionSelector = userSectionSelector;
            UserTabSelector = userTabSelector;
        }
        protected virtual void Start()
        {
            UserSectionSelector.Selected += OnSectionSelected;
            if (UserSectionSelector.CurrentValue != null)
                OnSectionSelected(UserSectionSelector, UserSectionSelector.CurrentValue);

            UserTabSelector.Selected += OnTabSelected;
            if (UserTabSelector.CurrentValue != null)
                OnTabSelected(UserTabSelector, UserTabSelector.CurrentValue);
        }

        protected UserSection Section { get; set; }
        protected virtual void OnSectionSelected(object sender, UserSectionSelectedEventArgs eventArgs)
        {
            if (Section == eventArgs.SelectedSection)
                return;
            Section = eventArgs.SelectedSection;

            foreach (var tabButton in TabButtons)
                TabButtonPool.Despawn(tabButton.Value);
            TabButtons.Clear();

            foreach (var tab in Section.Data.Tabs)
                AddButton(Section.GetTab(tab.Key));
        }

        protected Dictionary<UserTab, BaseReaderTabToggle> TabButtons { get; } = new Dictionary<UserTab, BaseReaderTabToggle>();
        protected void AddButton(UserTab userTab)
        {
            var tabButton = TabButtonPool.Spawn();
            tabButton.transform.SetParent(TabButtonsParent);
            tabButton.transform.SetAsLastSibling();
            tabButton.SetToggleGroup(TabsToggleGroup);
            tabButton.Display(userTab);
            tabButton.Selected += () => OnSelected(userTab);
            TabButtons.Add(userTab, tabButton);
        }

        protected UserTab CurrentTab { get; set; }
        protected void OnSelected(UserTab tab)
        {
            if (CurrentTab != tab) {
                CurrentTab = tab;
                var selectedArgs = new UserTabSelectedEventArgs(tab, ChangeType.JumpTo);
                UserTabSelector.Select(this, selectedArgs);
            }

            var tabButtonTransform = (RectTransform)TabButtons[tab].transform;
            EnsureButtonIsShowing(tab, tabButtonTransform);
            NextFrame.Function(() => NextFrame.Function(() => EnsureButtonIsShowing(tab, tabButtonTransform)));
        }

        protected virtual void EnsureButtonIsShowing(UserTab tab, RectTransform tabButtonTransform)
        {
            if (tab == CurrentTab && TabButtonsScroll != null)
                TabButtonsScroll.EnsureChildIsShowing((RectTransform)TabButtons[tab].transform);
        }

        protected virtual void OnTabSelected(object sender, UserTabSelectedEventArgs eventArgs)
        {
            if ((object)sender == this)
                return;

            CurrentTab = eventArgs.SelectedTab;
            TabButtons[CurrentTab].Select();
        }
    }
}
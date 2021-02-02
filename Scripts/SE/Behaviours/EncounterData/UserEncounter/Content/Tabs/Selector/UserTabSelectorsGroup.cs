using ClinicalTools.UI;
using ClinicalTools.UI.Extensions;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class UserTabSelectorsGroup<T> : MonoBehaviour
        where T : BaseSelectableUserTabBehaviour
    {
        public virtual Transform TabButtonsParent { get => tabButtonsParent; set => tabButtonsParent = value; }
        [SerializeField] private Transform tabButtonsParent;
        public virtual ScrollRect TabButtonsScroll { get => tabButtonsScroll; set => tabButtonsScroll = value; }
        [SerializeField] private ScrollRect tabButtonsScroll;

        protected SceneMonoMemoryPool<T> TabButtonPool { get; set; }
        protected ISelector<UserSectionSelectedEventArgs> UserSectionSelector { get; set; }
        protected ISelector<UserTabSelectedEventArgs> UserTabSelector { get; set; }
        [Inject]
        public virtual void Inject(
            SceneMonoMemoryPool<T> tabButtonPool,
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

        protected Dictionary<UserTab, T> TabButtons { get; } = new Dictionary<UserTab, T>();
        protected void AddButton(UserTab userTab)
        {
            var tabButton = CreateNewTabButton();
            tabButton.Initialize(userTab);
            TabButtons.Add(userTab, tabButton);
        }

        protected virtual T CreateNewTabButton()
        {
            var tabButton = TabButtonPool.Spawn();
            tabButton.transform.SetParent(TabButtonsParent);
            tabButton.transform.SetAsLastSibling();
            return tabButton;
        }


        protected UserTab CurrentTab { get; set; }
        protected virtual void OnSelected(UserTab tab)
        {
            if (CurrentTab == tab)
                return;

            CurrentTab = tab;

            var tabButtonTransform = (RectTransform)TabButtons[CurrentTab].transform;
            EnsureCurrentTabIsShowing(tabButtonTransform);
            NextFrame.Function(() => NextFrame.Function(() => EnsureCurrentTabIsShowing(tabButtonTransform)));
        }

        protected virtual void EnsureCurrentTabIsShowing(RectTransform tabButtonTransform)
        {
            if (TabButtonsScroll != null)
                TabButtonsScroll.EnsureChildIsShowing((RectTransform)TabButtons[CurrentTab].transform);
        }

        protected virtual void OnTabSelected(object sender, UserTabSelectedEventArgs eventArgs)
        {
            if ((object)sender == this)
                return;

            OnSelected(eventArgs.SelectedTab);
        }
    }
}
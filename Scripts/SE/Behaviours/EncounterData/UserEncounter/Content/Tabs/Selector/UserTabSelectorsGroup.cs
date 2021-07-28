using ClinicalTools.UI;
using ClinicalTools.UI.Extensions;
using System;
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

        public bool ShowTableOfContents { get => showTableOfContents; set => showTableOfContents = value; }
        [SerializeField] private bool showTableOfContents = true;

        protected SceneMonoMemoryPool<T> TabButtonPool { get; set; }
        protected ISelectedListener<UserSectionSelectedEventArgs> SectionSelectedListener { get; set; }
        protected ISelectedListener<UserTabSelectedEventArgs> TabSelectedListener { get; set; }
        [Inject]
        public virtual void Inject(
            SceneMonoMemoryPool<T> tabButtonPool,
            ISelectedListener<UserSectionSelectedEventArgs> sectionSelectedListener,
            ISelectedListener<UserTabSelectedEventArgs> tabSelectedListener)
        {
            TabButtonPool = tabButtonPool;

            SectionSelectedListener = sectionSelectedListener;
            TabSelectedListener = tabSelectedListener;
        }

        protected virtual void Start()
        {
            SectionSelectedListener.Selected += OnSectionSelected;
            if (SectionSelectedListener.CurrentValue != null)
                OnSectionSelected(SectionSelectedListener, SectionSelectedListener.CurrentValue);

            TabSelectedListener.Selected += OnTabSelected;
            if (TabSelectedListener.CurrentValue != null)
                OnTabSelected(TabSelectedListener, TabSelectedListener.CurrentValue);
        }

        protected UserSection Section { get; set; }
        protected virtual void OnSectionSelected(object sender, UserSectionSelectedEventArgs eventArgs)
        {
            if (Section == eventArgs.SelectedSection)
                return;
            Section = eventArgs.SelectedSection;

            ClearButtons();

            foreach (var tab in Section.Tabs.Values)
                AddButton(tab);
        }

        protected virtual void ClearButtons()
        {
            foreach (var tabButton in TabButtons)
                DespawnButton(tabButton.Value);
            TabButtons.Clear();
        }

        protected virtual void DespawnButton(T tabButton) => TabButtonPool.Despawn(tabButton);

        protected Dictionary<UserTab, T> TabButtons { get; } = new Dictionary<UserTab, T>();
        protected void AddButton(UserTab userTab)
        {
            if (!ShowTableOfContents && userTab.Data.Type.Equals("Table Of Contents", StringComparison.InvariantCultureIgnoreCase))
                return;

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

            if (!TabButtons.ContainsKey(CurrentTab))
                return;
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

        protected virtual void OnDestroy()
        {
            SectionSelectedListener.Selected -= OnSectionSelected;
            TabSelectedListener.Selected -= OnTabSelected;
        }
    }
}
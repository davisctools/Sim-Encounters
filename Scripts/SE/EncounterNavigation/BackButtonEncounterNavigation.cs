using ClinicalTools.UI;
using System;
using System.Collections.Generic;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class BackButtonEncounterNavigation
    {
        protected ISelector<UserSectionSelectedEventArgs> SectionSelector { get; set; }
        protected ISelectedListener<UserSectionSelectedEventArgs> SectionSelectedListener { get; set; }
        protected ISelector<UserTabSelectedEventArgs> TabSelector { get; set; }
        protected ISelectedListener<UserTabSelectedEventArgs> TabSelectedListener { get; set; }
        protected AndroidBackButton BackButton { get; set; }
        [Inject]
        public virtual void Inject(
            ISelector<UserSectionSelectedEventArgs> sectionSelector,
            ISelectedListener<UserSectionSelectedEventArgs> sectionSelectedListener,
            ISelector<UserTabSelectedEventArgs> tabSelector,
            ISelectedListener<UserTabSelectedEventArgs> tabSelectedListener,
            AndroidBackButton backButton)
        {
            SectionSelector = sectionSelector;
            SectionSelectedListener = sectionSelectedListener;

            TabSelector = tabSelector;

            TabSelectedListener = tabSelectedListener;
            TabSelectedListener.Selected += OnTabSelected;
            if (TabSelectedListener.CurrentValue != null)
                OnTabSelected(this, TabSelectedListener.CurrentValue);

            BackButton = backButton;
            BackButton.Register(OnBackButton);
        }

        protected UserSection CurrentSection => SectionSelectedListener.CurrentValue.SelectedSection;

        protected Stack<Tuple<UserSection, UserTabSelectedEventArgs>> TabChangedEvents { get; } = new Stack<Tuple<UserSection, UserTabSelectedEventArgs>>();
        protected Tuple<UserSection, UserTab> LastTab { get; set;  }
        protected virtual void OnTabSelected(object sender, UserTabSelectedEventArgs eventArgs)
        {
            if (sender == this || LastTab?.Item2 == eventArgs.SelectedTab) {
                LastTab = new Tuple<UserSection, UserTab>(CurrentSection, eventArgs.SelectedTab);
                return;
            }

            if (LastTab != null) {
                var tabEventArgs = new UserTabSelectedEventArgs(LastTab.Item2, GetTabChangeType(eventArgs.ChangeType));
                TabChangedEvents.Push(new Tuple<UserSection, UserTabSelectedEventArgs>(LastTab.Item1, tabEventArgs));
            }

            LastTab = new Tuple<UserSection, UserTab>(CurrentSection, eventArgs.SelectedTab);
        }

        protected virtual ChangeType GetTabChangeType(ChangeType changeType)
        {
            if (changeType == ChangeType.Next)
                return ChangeType.Previous;
            else if (changeType == ChangeType.Previous)
                return ChangeType.Next;
            else
                return changeType;
        }

        protected virtual void OnBackButton()
        {
            BackButton.Register(OnBackButton);

            if (TabChangedEvents.Count == 0)
                return;

            var tabChangedEvent = TabChangedEvents.Pop();
            if (tabChangedEvent.Item1 == CurrentSection) {
                TabSelector.Select(this, tabChangedEvent.Item2);
                return;
            }

            tabChangedEvent.Item1.Data.SetCurrentTab(tabChangedEvent.Item2.SelectedTab.Data);
            SectionSelector.Select(this, new UserSectionSelectedEventArgs(tabChangedEvent.Item1, tabChangedEvent.Item2.ChangeType));
        }
    }
}
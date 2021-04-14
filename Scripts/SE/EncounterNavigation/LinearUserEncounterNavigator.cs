using System;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class LinearUserEncounterNavigator : ILinearEncounterNavigator
    {
        protected ISelector<UserEncounterSelectedEventArgs> UserEncounterSelector { get; set; }
        protected ISelector<UserSectionSelectedEventArgs> UserSectionSelector { get; set; }
        protected ISelector<UserTabSelectedEventArgs> UserTabSelector { get; set; }

        public event SelectedHandler<UserTabSelectedEventArgs> EncounterTabPositionChanged;
        public UserTabSelectedEventArgs CurrentTab => UserTabSelector.CurrentValue;

        [Inject]
        public virtual void Inject(
            ISelector<UserEncounterSelectedEventArgs> userEncounterSelector,
            ISelector<UserSectionSelectedEventArgs> userSectionSelector,
            ISelector<UserTabSelectedEventArgs> userTabSelector)
        {
            UserEncounterSelector = userEncounterSelector;
            UserEncounterSelector.Selected += OnEncounterSelected;
            if (UserEncounterSelector.CurrentValue != null)
                OnEncounterSelected(UserEncounterSelector, UserEncounterSelector.CurrentValue);

            UserSectionSelector = userSectionSelector;
            UserSectionSelector.Selected += OnSectionSelected;
            if (UserSectionSelector.CurrentValue != null)
                OnSectionSelected(UserSectionSelector, UserSectionSelector.CurrentValue);

            UserTabSelector = userTabSelector;
            UserTabSelector.Selected += OnTabSelected;
        }

        protected UserEncounter UserEncounter { get; set; }
        protected EncounterContentData Content => UserEncounter.Data.Content;
        protected Section CurrentSection => Content.Sections[Content.CurrentSectionIndex].Value;
        protected virtual void OnEncounterSelected(object sender, UserEncounterSelectedEventArgs eventArgs)
            => UserEncounter = eventArgs.Encounter;


        protected UserSection CurrentUserSection { get; set; }
        protected virtual void OnSectionSelected(object sender, UserSectionSelectedEventArgs eventArgs)
            => CurrentUserSection = eventArgs.SelectedSection;
        protected virtual void OnTabSelected(object sender, UserTabSelectedEventArgs eventArgs)
            => EncounterTabPositionChanged?.Invoke(sender, eventArgs);

        public virtual bool HasNext() => HasNextSection() || HasNextTab();
        protected virtual bool HasNextSection() => Content.CurrentSectionIndex + 1 < Content.Sections.Count;
        protected virtual bool HasNextTab() => CurrentSection.CurrentTabIndex + 1 < CurrentSection.Tabs.Count;
        public virtual void GoToNext()
        {
            if (HasNextTab())
                GoToNextTab();
            else if (HasNextSection())
                GoToNextSection();
        }

        public virtual bool HasPrevious() => HasPreviousSection() || HasPreviousTab();
        protected virtual bool HasPreviousSection() => Content.CurrentSectionIndex != 0;
        protected virtual bool HasPreviousTab() => CurrentSection.CurrentTabIndex != 0;
        public virtual void GoToPrevious()
        {
            if (HasPreviousTab())
                GoToPreviousTab();
            else if (HasPreviousSection())
                GoToPreviousSection();
        }

        protected virtual void GoToNextSection()
        {
            var nextSectionIndex = Content.CurrentSectionIndex + 1;
            var section = Content.Sections[nextSectionIndex].Value;
            section.CurrentTabIndex = 0;
            GoToSection(nextSectionIndex, ChangeType.Next);
        }
        protected virtual void GoToPreviousSection()
        {
            var previousSectionIndex = Content.CurrentSectionIndex - 1;
            var section = Content.Sections[previousSectionIndex].Value;
            section.CurrentTabIndex = section.Tabs.Count - 1;
            GoToSection(previousSectionIndex, ChangeType.Previous);
        }
        protected virtual void GoToSection(int sectionIndex, ChangeType changeType)
        {
            var nextSectionKey = Content.Sections[sectionIndex].Key;
            var nextSection = UserEncounter.GetSection(nextSectionKey);
            var selectedArgs = new UserSectionSelectedEventArgs(nextSection, changeType);
            UserSectionSelector.Select(this, selectedArgs);
        }

        protected virtual void GoToNextTab()
            => GoToTab(CurrentSection.CurrentTabIndex + 1, ChangeType.Next);
        protected virtual void GoToPreviousTab()
            => GoToTab(CurrentSection.CurrentTabIndex - 1, ChangeType.Previous);
        protected virtual void GoToTab(int tabIndex, ChangeType changeType)
        {
            var section = CurrentSection;
            var nextTabKey = section.Tabs[tabIndex].Key;
            var nextTab = CurrentUserSection.GetTab(nextTabKey);
            var selectedArgs = new UserTabSelectedEventArgs(nextTab, changeType);
            UserTabSelector.Select(this, selectedArgs);
        }
    }
}
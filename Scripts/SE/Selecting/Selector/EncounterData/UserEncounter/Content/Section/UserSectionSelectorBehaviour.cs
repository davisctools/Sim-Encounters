﻿namespace ClinicalTools.SimEncounters
{
    public class UserSectionSelectorBehaviour : UserTabSelectorBehaviour,
        ISelectedListener<UserSectionSelectedEventArgs>,
        ISelectedListener<SectionSelectedEventArgs>
    {
        protected UserSectionSelectedEventArgs UserSectionValue { get; set; }
        UserSectionSelectedEventArgs ISelectedListener<UserSectionSelectedEventArgs>.CurrentValue => UserSectionValue;
        public event SelectedHandler<UserSectionSelectedEventArgs> UserSectionSelected;
        event SelectedHandler<UserSectionSelectedEventArgs> ISelectedListener<UserSectionSelectedEventArgs>.Selected {
            add => UserSectionSelected += value;
            remove => UserSectionSelected -= value;
        }

        protected SectionSelectedEventArgs SectionValue { get; set; }
        SectionSelectedEventArgs ISelectedListener<SectionSelectedEventArgs>.CurrentValue => SectionValue;
        public event SelectedHandler<SectionSelectedEventArgs> SectionSelected;
        event SelectedHandler<SectionSelectedEventArgs> ISelectedListener<SectionSelectedEventArgs>.Selected {
            add => SectionSelected += value;
            remove => SectionSelected -= value;
        }

        public virtual void Display(object sender, UserSectionSelectedEventArgs eventArgs)
        {
            UserSectionValue = eventArgs;
            UserSectionSelected?.Invoke(sender, UserSectionValue);

            SectionValue = new SectionSelectedEventArgs(UserSectionValue.SelectedSection.Data);
            SectionSelected?.Invoke(sender, SectionValue);
        }

        public override void Display(object sender, UserTabSelectedEventArgs eventArgs)
        {
            if (eventArgs.ChangeType != ChangeType.Inactive)
                SectionValue.SelectedSection.SetCurrentTab(eventArgs.SelectedTab.Data);
            base.Display(sender, eventArgs);
        }
    }
}
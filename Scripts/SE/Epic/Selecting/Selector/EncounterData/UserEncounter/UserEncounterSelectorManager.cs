using System.Diagnostics;

namespace ClinicalTools.SimEncounters
{
    public class UserEncounterSelectorManager :
        ISelector<UserEncounterSelectedEventArgs>,
        ISelector<UserSectionSelectedEventArgs>,
        ISelector<UserTabSelectedEventArgs>,
        ISelectedListener<EncounterSelectedEventArgs>,
        ISelectedListener<SectionSelectedEventArgs>,
        ISelectedListener<TabSelectedEventArgs>,
        ISelectedListener<EncounterMetadataSelectedEventArgs>
    {
        protected UserEncounterSelectedEventArgs UserEncounterValue { get; set; }
        UserEncounterSelectedEventArgs ISelectedListener<UserEncounterSelectedEventArgs>.CurrentValue => UserEncounterValue;
        public event SelectedHandler<UserEncounterSelectedEventArgs> UserEncounterSelected;
        event SelectedHandler<UserEncounterSelectedEventArgs> ISelectedListener<UserEncounterSelectedEventArgs>.Selected {
            add => UserEncounterSelected += value;
            remove => UserEncounterSelected -= value;
        }

        protected EncounterSelectedEventArgs EncounterValue { get; set; }
        EncounterSelectedEventArgs ISelectedListener<EncounterSelectedEventArgs>.CurrentValue => EncounterValue;
        public event SelectedHandler<EncounterSelectedEventArgs> EncounterSelected;
        event SelectedHandler<EncounterSelectedEventArgs> ISelectedListener<EncounterSelectedEventArgs>.Selected {
            add => EncounterSelected += value;
            remove => EncounterSelected -= value;
        }

        protected EncounterMetadataSelectedEventArgs MetadataValue { get; set; }
        EncounterMetadataSelectedEventArgs ISelectedListener<EncounterMetadataSelectedEventArgs>.CurrentValue => MetadataValue;
        public event SelectedHandler<EncounterMetadataSelectedEventArgs> MetadataSelected;
        event SelectedHandler<EncounterMetadataSelectedEventArgs> ISelectedListener<EncounterMetadataSelectedEventArgs>.Selected {
            add => MetadataSelected += value;
            remove => MetadataSelected -= value;
        }

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

        protected UserTabSelectedEventArgs UserTabValue { get; set; }
        UserTabSelectedEventArgs ISelectedListener<UserTabSelectedEventArgs>.CurrentValue => UserTabValue;
        public event SelectedHandler<UserTabSelectedEventArgs> UserTabSelected;
        event SelectedHandler<UserTabSelectedEventArgs> ISelectedListener<UserTabSelectedEventArgs>.Selected {
            add => UserTabSelected += value;
            remove => UserTabSelected -= value;
        }

        protected TabSelectedEventArgs TabValue { get; set; }
        TabSelectedEventArgs ISelectedListener<TabSelectedEventArgs>.CurrentValue => TabValue;
        public event SelectedHandler<TabSelectedEventArgs> TabSelected;
        event SelectedHandler<TabSelectedEventArgs> ISelectedListener<TabSelectedEventArgs>.Selected {
            add => TabSelected += value;
            remove => TabSelected -= value;
        }


        public virtual void Select(object sender, UserEncounterSelectedEventArgs eventArgs)
        {
            if (UserEncounterValue == eventArgs)
                return;

            UserEncounterValue = eventArgs;
            UserEncounterSelected?.Invoke(sender, UserEncounterValue);

            EncounterValue = new EncounterSelectedEventArgs(UserEncounterValue.Encounter.Data);
            EncounterSelected?.Invoke(sender, EncounterValue);

            MetadataValue = new EncounterMetadataSelectedEventArgs(EncounterValue.Encounter.Metadata);
            MetadataSelected?.Invoke(sender, MetadataValue);
            Select(sender, new UserSectionSelectedEventArgs(UserEncounterValue.Encounter.GetCurrentSection(), ChangeType.JumpTo));
        }

        public virtual void Select(object sender, UserSectionSelectedEventArgs eventArgs)
        {
            if (UserSectionValue == eventArgs)
                return;

            var stopwatch = Stopwatch.StartNew();
            var userSection = eventArgs.SelectedSection;

            UserSectionValue = eventArgs;
            EncounterValue.Encounter.Content.NonImageContent.SetCurrentSection(userSection.Data);
            UserSectionSelected?.Invoke(sender, UserSectionValue);

            SectionValue = new SectionSelectedEventArgs(userSection.Data);
            SectionSelected?.Invoke(sender, SectionValue);
            Select(sender, new UserTabSelectedEventArgs(userSection.GetCurrentTab(), eventArgs.ChangeType));
            UnityEngine.Debug.LogWarning($"A. SECTION: {stopwatch.ElapsedMilliseconds}");
        }

        public virtual void Select(object sender, UserTabSelectedEventArgs eventArgs)
        {
            if (UserTabValue == eventArgs)
                return;

            var stopwatch = Stopwatch.StartNew();
            UserTabValue = eventArgs;
            SectionValue.SelectedSection.SetCurrentTab(UserTabValue.SelectedTab.Data);
            UserTabSelected?.Invoke(sender, UserTabValue);

            TabValue = new TabSelectedEventArgs(UserTabValue.SelectedTab.Data);
            TabSelected?.Invoke(sender, TabValue);
            UnityEngine.Debug.LogWarning($"A. TAB: {stopwatch.ElapsedMilliseconds}");
        }
    }
}
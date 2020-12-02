namespace ClinicalTools.SimEncounters
{
    public class EncounterSelectorManager :
        ISelector<EncounterSelectedEventArgs>,
        ISelector<SectionSelectedEventArgs>,
        ISelector<TabSelectedEventArgs>,
        ISelectedListener<EncounterMetadataSelectedEventArgs>
    {
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

        protected SectionSelectedEventArgs SectionValue { get; set; }
        SectionSelectedEventArgs ISelectedListener<SectionSelectedEventArgs>.CurrentValue => SectionValue;
        public event SelectedHandler<SectionSelectedEventArgs> SectionSelected;
        event SelectedHandler<SectionSelectedEventArgs> ISelectedListener<SectionSelectedEventArgs>.Selected {
            add => SectionSelected += value;
            remove => SectionSelected -= value;
        }

        protected TabSelectedEventArgs TabValue { get; set; }
        TabSelectedEventArgs ISelectedListener<TabSelectedEventArgs>.CurrentValue => TabValue;
        public event SelectedHandler<TabSelectedEventArgs> TabSelected;
        event SelectedHandler<TabSelectedEventArgs> ISelectedListener<TabSelectedEventArgs>.Selected {
            add => TabSelected += value;
            remove => TabSelected -= value;
        }

        public virtual void Select(object sender, EncounterSelectedEventArgs eventArgs)
        {
            if (EncounterValue == eventArgs)
                return;

            EncounterValue = eventArgs;
            EncounterSelected?.Invoke(sender, EncounterValue);

            var encounter = EncounterValue.Encounter;
            MetadataValue = new EncounterMetadataSelectedEventArgs(encounter.Metadata);
            MetadataSelected?.Invoke(sender, MetadataValue);

            if (encounter.Content.NonImageContent.Sections.Count > 0)
                Select(sender, new SectionSelectedEventArgs(encounter.Content.NonImageContent.GetCurrentSection()));
        }

        public virtual void Select(object sender, SectionSelectedEventArgs eventArgs)
        {
            if (SectionValue == eventArgs)
                return;
                        
            SectionValue = eventArgs;
            var section = eventArgs.SelectedSection;
            EncounterValue.Encounter.Content.NonImageContent.SetCurrentSection(section);
            SectionSelected?.Invoke(sender, SectionValue);
            
            if (eventArgs.SelectedSection.Tabs.Count > 0)
                Select(sender, new TabSelectedEventArgs(eventArgs.SelectedSection.GetCurrentTab()));
        }

        public virtual void Select(object sender, TabSelectedEventArgs eventArgs)
        {
            if (TabValue == eventArgs)
                return;

            TabValue = eventArgs;
            SectionValue.SelectedSection.SetCurrentTab(TabValue.SelectedTab);
            TabSelected?.Invoke(sender, TabValue);
        }
    }
}
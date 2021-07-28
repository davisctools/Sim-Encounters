namespace ClinicalTools.SimEncounters
{
    public class MenuEncounterOverview : BaseMenuEncounterOverview,
        ISelectedListener<MenuEncounterSelectedEventArgs>,
        ISelectedListener<EncounterMetadataSelectedEventArgs>
    {
        public MenuEncounterSelectedEventArgs CurrentValue { get; protected set; }
        protected EncounterMetadataSelectedEventArgs CurrentMetadataValue { get; set; }
        EncounterMetadataSelectedEventArgs ISelectedListener<EncounterMetadataSelectedEventArgs>.CurrentValue => CurrentMetadataValue;

        public event SelectedHandler<MenuEncounterSelectedEventArgs> Selected;
        public event SelectedHandler<EncounterMetadataSelectedEventArgs> MetadataSelected;
        event SelectedHandler<EncounterMetadataSelectedEventArgs> ISelectedListener<EncounterMetadataSelectedEventArgs>.Selected {
            add => MetadataSelected += value;
            remove => MetadataSelected -= value;
        }

        public override void Display(object sender, MenuEncounterSelectedEventArgs eventArgs)
        {
            gameObject.SetActive(true);

            CurrentValue = eventArgs;
            Selected?.Invoke(sender, eventArgs);
            CurrentMetadataValue = new EncounterMetadataSelectedEventArgs(eventArgs.Encounter.GetLatestMetadata());
            MetadataSelected?.Invoke(sender, CurrentMetadataValue);
        }

        public override void Hide() => gameObject.SetActive(false);
    }
}
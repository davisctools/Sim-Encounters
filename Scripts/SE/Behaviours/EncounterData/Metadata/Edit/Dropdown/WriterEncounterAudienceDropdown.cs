namespace ClinicalTools.SimEncounters
{
    public class WriterEncounterAudienceDropdown : WriterMetadataDropdown
    {
        protected override void OnMetadataSelected(object sender, EncounterMetadataSelectedEventArgs eventArgs)
            => SetValue(eventArgs.Metadata.Audience);

        protected override void Serialize(EncounterMetadata metadata) => metadata.Audience = GetValue();
    }
}
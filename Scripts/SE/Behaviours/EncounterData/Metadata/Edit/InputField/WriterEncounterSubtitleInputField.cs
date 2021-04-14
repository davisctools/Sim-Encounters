namespace ClinicalTools.SimEncounters
{
    public class WriterEncounterSubtitleInputField : WriterMetadataInputField
    {
        protected override void OnMetadataSelected(object sender, EncounterMetadataSelectedEventArgs eventArgs)
            => InputField.text = eventArgs.Metadata.Subtitle;
        protected override void Serialize(OldEncounterMetadata metadata) => metadata.Subtitle = InputField.text;
    }
}
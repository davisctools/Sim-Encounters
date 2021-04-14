namespace ClinicalTools.SimEncounters
{
    public class WriterEncounterDescriptionInputField : WriterMetadataInputField
    {
        protected override void OnMetadataSelected(object sender, EncounterMetadataSelectedEventArgs eventArgs)
            => InputField.text = eventArgs.Metadata.Description;
        protected override void Serialize(OldEncounterMetadata metadata) => metadata.Description = InputField.text;
    }
}
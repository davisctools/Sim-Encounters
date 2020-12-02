namespace ClinicalTools.SimEncounters
{
    public class WriterEncounterDescriptionInputField : WriterMetadataInputField
    {
        protected override void OnMetadataSelected(object sender, EncounterMetadataSelectedEventArgs eventArgs)
            => InputField.text = eventArgs.Metadata.Description;
        protected override void Serialize(EncounterMetadata metadata) => metadata.Description = InputField.text;
    }
}
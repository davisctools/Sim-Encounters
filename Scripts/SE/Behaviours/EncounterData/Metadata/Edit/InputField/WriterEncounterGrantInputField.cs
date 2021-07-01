namespace ClinicalTools.SimEncounters
{
    public class WriterEncounterGrantInputField : WriterMetadataInputField
    {
        protected override void OnMetadataSelected(object sender, EncounterMetadataSelectedEventArgs eventArgs)
            => InputField.text = eventArgs.Metadata.GrantInfo;
        protected override void Serialize(EncounterMetadata metadata) => metadata.GrantInfo = InputField.text;
    }
}
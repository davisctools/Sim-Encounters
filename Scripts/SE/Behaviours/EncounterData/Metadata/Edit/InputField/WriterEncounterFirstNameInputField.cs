namespace ClinicalTools.SimEncounters
{
    public class WriterEncounterFirstNameInputField : WriterMetadataInputField
    {
        protected override void OnMetadataSelected(object sender, EncounterMetadataSelectedEventArgs eventArgs)
            => InputField.text = ((INamed)eventArgs.Metadata).Name.FirstName;
        protected override void Serialize(EncounterMetadata metadata)
            => ((INamed)metadata).Name.FirstName = InputField.text;
    }
}
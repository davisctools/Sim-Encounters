namespace ClinicalTools.SimEncounters
{
    public class WriterEncounterLastNameInputField : WriterMetadataInputField
    {
        protected override void OnMetadataSelected(object sender, EncounterMetadataSelectedEventArgs eventArgs)
            => InputField.text = ((INamed)eventArgs.Metadata).Name.LastName;
        protected override void Serialize(OldEncounterMetadata metadata)
            => ((INamed)metadata).Name.LastName = InputField.text;
    }
}
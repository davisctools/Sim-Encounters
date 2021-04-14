namespace ClinicalTools.SimEncounters
{
    public class WriterEncounterUrlInputField : WriterMetadataInputField
    {
        protected override void OnMetadataSelected(object sender, EncounterMetadataSelectedEventArgs eventArgs)
            => InputField.text = ((IWebCompletion)eventArgs.Metadata).Url;
        protected override void Serialize(OldEncounterMetadata metadata)
            => ((IWebCompletion)metadata).Url = InputField.text;
    }
}
namespace ClinicalTools.SimEncounters
{
    public class WriterEncounterCompletionCodeInputField : WriterMetadataInputField
    {
        protected override void OnMetadataSelected(object sender, EncounterMetadataSelectedEventArgs eventArgs)
            => InputField.text = ((IWebCompletion)eventArgs.Metadata).CompletionCode;
        protected override void Serialize(OldEncounterMetadata metadata)
            => ((IWebCompletion)metadata).CompletionCode = InputField.text;
    }
}
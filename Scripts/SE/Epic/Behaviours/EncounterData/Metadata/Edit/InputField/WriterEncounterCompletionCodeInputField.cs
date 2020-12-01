namespace ClinicalTools.SimEncounters
{
    public class WriterEncounterCompletionCodeInputField : WriterMetadataInputField
    {
        protected override void OnMetadataSelected(object sender, EncounterMetadataSelectedEventArgs eventArgs)
            => InputField.text = ((IWebCompletion)eventArgs.Metadata).CompletionCode;
        protected override void Serialize(EncounterMetadata metadata)
            => ((IWebCompletion)metadata).CompletionCode = InputField.text;
    }
}
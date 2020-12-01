namespace ClinicalTools.SimEncounters
{
    public class WriterEncounterCategoriesInputField : WriterMetadataInputField
    {
        protected override void OnMetadataSelected(object sender, EncounterMetadataSelectedEventArgs eventArgs)
            => InputField.text = string.Join("; ", eventArgs.Metadata.Categories);
        protected override void Serialize(EncounterMetadata metadata)
        {
            var categories = InputField.text.Split(';');
            metadata.Categories.Clear();
            foreach (var category in categories) {
                if (!string.IsNullOrWhiteSpace(category))
                    metadata.Categories.Add(category.Trim());
            }
        }
    }
}
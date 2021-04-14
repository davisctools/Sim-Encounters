namespace ClinicalTools.SimEncounters
{
    public class WriterEncounterDifficultyDropdown : WriterMetadataDropdown
    {
        protected override void OnMetadataSelected(object sender, EncounterMetadataSelectedEventArgs eventArgs)
            => Dropdown.value = (int)eventArgs.Metadata.Difficulty;

        protected override void Serialize(OldEncounterMetadata metadata)
            => metadata.Difficulty = (EncounterDifficulty)Dropdown.value;
    }
}
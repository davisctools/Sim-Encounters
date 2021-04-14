namespace ClinicalTools.SimEncounters
{
    public interface IMetadataWriter
    {
        void Save(User user, OldEncounterMetadata metadata);
    }
}

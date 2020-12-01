namespace ClinicalTools.SimEncounters
{
    public interface IMetadataWriter
    {
        void Save(User user, EncounterMetadata metadata);
    }
}

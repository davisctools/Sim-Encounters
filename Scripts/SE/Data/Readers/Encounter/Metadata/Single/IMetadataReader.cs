namespace ClinicalTools.SimEncounters
{
    public interface IMetadataReader
    {
        WaitableTask<EncounterMetadata> GetMetadata(User user, EncounterMetadata metadata);
    }
}

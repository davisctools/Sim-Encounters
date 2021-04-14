namespace ClinicalTools.SimEncounters
{
    public interface IMetadataReader
    {
        WaitableTask<OldEncounterMetadata> GetMetadata(User user, OldEncounterMetadata metadata);
    }
}

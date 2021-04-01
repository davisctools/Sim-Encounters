namespace ClinicalTools.SimEncounters
{
    public interface INonImageContentReader
    {
        WaitableTask<EncounterContent> GetNonImageContent(User user, EncounterMetadata metadata);
    }
}
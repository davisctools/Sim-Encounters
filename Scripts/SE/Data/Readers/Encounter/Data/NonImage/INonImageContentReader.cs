namespace ClinicalTools.SimEncounters
{
    public interface INonImageContentReader
    {
        WaitableTask<EncounterNonImageContent> GetNonImageContent(User user, EncounterMetadata metadata);
    }
}
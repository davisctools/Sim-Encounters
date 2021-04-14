namespace ClinicalTools.SimEncounters
{
    public interface IEncounterImagesJsonRetriever
    {
        WaitableTask<string> GetImagesJson(User user, OldEncounterMetadata metadata);
    }
}
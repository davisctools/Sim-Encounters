namespace ClinicalTools.SimEncounters
{
    public interface IImageContentReader
    {
        WaitableTask<LegacyEncounterImageContent> GetImageData(User user, OldEncounterMetadata metadata);
    }
}
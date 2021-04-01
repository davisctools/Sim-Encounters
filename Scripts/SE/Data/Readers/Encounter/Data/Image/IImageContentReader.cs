namespace ClinicalTools.SimEncounters
{
    public interface IImageContentReader
    {
        WaitableTask<LegacyEncounterImageContent> GetImageData(User user, EncounterMetadata metadata);
    }
}
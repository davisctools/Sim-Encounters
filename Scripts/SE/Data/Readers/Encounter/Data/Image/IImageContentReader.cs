namespace ClinicalTools.SimEncounters
{
    public interface IImageContentReader
    {
        WaitableTask<EncounterImageContent> GetImageData(User user, EncounterMetadata metadata);
    }
}
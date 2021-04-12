namespace ClinicalTools.SimEncounters
{
    public interface IEncounterImageUploader
    {
        WaitableTask<EncounterImage> UploadImage(User user, Encounter encounter);
    }
}
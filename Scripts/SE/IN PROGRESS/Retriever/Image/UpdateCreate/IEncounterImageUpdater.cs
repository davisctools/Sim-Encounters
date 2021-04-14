namespace ClinicalTools.SimEncounters
{
    public interface IEncounterImageUpdater
    {
        WaitableTask<EncounterImage> UpdateImage(User user, ContentEncounter encounter, EncounterImage image);
    }
}
namespace ClinicalTools.SimEncounters
{
    public interface IEncounterImageUpdater
    {
        WaitableTask<EncounterImage> UpdateImage(User user, Encounter encounter, EncounterImage image);
    }
}
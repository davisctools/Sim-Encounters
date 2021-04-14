namespace ClinicalTools.SimEncounters
{
    public interface IEncounterImageSelector
    {
        WaitableTask<EncounterImage> SelectImage(User user, ContentEncounter encounter, string key);
    }
}
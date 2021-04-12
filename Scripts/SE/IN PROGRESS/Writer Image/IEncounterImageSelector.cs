namespace ClinicalTools.SimEncounters
{
    public interface IEncounterImageSelector
    {
        WaitableTask<EncounterImage> SelectImage(User user, Encounter encounter, string key);
    }
}
namespace ClinicalTools.SimEncounters
{
    public interface IEncounterImageSelector
    {
        WaitableTask<string> SelectImage(User user, Encounter encounter, string key);
    }
}
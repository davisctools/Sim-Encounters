namespace ClinicalTools.SimEncounters
{
    public interface IDeleteEncounterHandler
    {
        WaitableTask Delete(User user, MenuEncounter encounter);
    }
}
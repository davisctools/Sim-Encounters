namespace ClinicalTools.SimEncounters
{
    public interface IEncounterWriter
    {
        WaitableTask Save(User user, Encounter encounter);
    }
}

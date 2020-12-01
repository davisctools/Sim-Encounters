namespace ClinicalTools.SimEncounters
{
    public interface IStatusWriter
    {
        WaitableTask WriteStatus(UserEncounter encounter);
    }
}
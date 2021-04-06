namespace ClinicalTools.SimEncounters
{
    public interface IEncounterWriter
    {
        WaitableTask Save(SaveEncounterParameters parameters);
    }
}

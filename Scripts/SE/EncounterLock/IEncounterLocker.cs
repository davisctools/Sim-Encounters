namespace ClinicalTools.SimEncounters
{
    public interface IEncounterLocker
    {
        WaitableTask LockEncounter(User user, OldEncounterMetadata metadata);
    }
}

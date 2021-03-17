namespace ClinicalTools.SimEncounters
{
    public interface IEncounterLocker
    {
        WaitableTask LockEncounter(User user, EncounterMetadata metadata);
    }
}

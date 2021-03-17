namespace ClinicalTools.SimEncounters
{
    public interface IEncounterUnlocker
    {
        WaitableTask UnlockEncounter(User user, EncounterMetadata metadata);
    }
}

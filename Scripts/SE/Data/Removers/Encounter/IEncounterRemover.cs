namespace ClinicalTools.SimEncounters
{
    public interface IEncounterRemover
    {
        WaitableTask Delete(User user, OldEncounterMetadata encounterMetadata);
    }
}

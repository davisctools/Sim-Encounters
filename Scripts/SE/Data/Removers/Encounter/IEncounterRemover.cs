namespace ClinicalTools.SimEncounters
{
    public interface IEncounterRemover
    {
        WaitableTask Delete(User user, EncounterMetadata encounterMetadata);
    }
}

namespace ClinicalTools.SimEncounters
{
    public interface IEncounterDataReader
    {
        WaitableTask<EncounterContent> GetEncounterData(User user, EncounterMetadata metadata);
    }
}
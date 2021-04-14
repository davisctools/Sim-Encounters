namespace ClinicalTools.SimEncounters
{
    public interface IEncounterDataReader
    {
        WaitableTask<EncounterContentData> GetEncounterData(User user, OldEncounterMetadata metadata);
    }
}
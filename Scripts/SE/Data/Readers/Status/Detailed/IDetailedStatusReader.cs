namespace ClinicalTools.SimEncounters
{
    public interface IDetailedStatusReader
    {
        WaitableTask<EncounterStatus> GetDetailedStatus(User user, OldEncounterMetadata metadata, EncounterBasicStatus basicStatus);
    }
}
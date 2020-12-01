namespace ClinicalTools.SimEncounters
{
    public interface IDetailedStatusReader
    {
        WaitableTask<EncounterStatus> GetDetailedStatus(User user, EncounterMetadata metadata, EncounterBasicStatus basicStatus);
    }
}
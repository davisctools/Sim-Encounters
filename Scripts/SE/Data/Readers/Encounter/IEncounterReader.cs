

namespace ClinicalTools.SimEncounters
{
    public interface IEncounterReader
    {
        WaitableTask<ContentEncounter> GetEncounter(User user, OldEncounterMetadata metadata, SaveType saveType);
    }
}
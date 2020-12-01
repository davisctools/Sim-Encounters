

namespace ClinicalTools.SimEncounters
{
    public interface IEncounterReader
    {
        WaitableTask<Encounter> GetEncounter(User user, EncounterMetadata metadata, SaveType saveType);
    }
}
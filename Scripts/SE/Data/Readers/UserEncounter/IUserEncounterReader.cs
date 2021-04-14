

namespace ClinicalTools.SimEncounters
{
    public interface IUserEncounterReader
    {
        WaitableTask<UserEncounter> GetUserEncounter(User user, OldEncounterMetadata metadata, EncounterBasicStatus basicStatus, SaveType saveType);
    }
}
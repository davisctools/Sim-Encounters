

namespace ClinicalTools.SimEncounters
{
    public interface IUserEncounterReader
    {
        WaitableTask<UserEncounter> GetUserEncounter(User user, EncounterMetadata metadata, EncounterBasicStatus basicStatus, SaveType saveType);
    }
}
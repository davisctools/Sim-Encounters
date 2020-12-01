

namespace ClinicalTools.SimEncounters
{
    public class UserEncounterReader : IUserEncounterReader
    {
        private readonly IEncounterReader dataReader;
        private readonly IDetailedStatusReader detailedStatusReader;
        public UserEncounterReader(IEncounterReader dataReader, IDetailedStatusReader detailedStatusReader)
        {
            this.dataReader = dataReader;
            this.detailedStatusReader = detailedStatusReader;
        }

        public WaitableTask<UserEncounter> GetUserEncounter(User user, EncounterMetadata metadata, EncounterBasicStatus basicStatus, SaveType saveType)
        {
            var encounterData = dataReader.GetEncounter(user, metadata, saveType);
            var detailedStatus = detailedStatusReader.GetDetailedStatus(user, metadata, basicStatus);

            var encounter = new WaitableTask<UserEncounter>();
            void processResults() => ProcessResults(user, encounter, encounterData, detailedStatus);
            encounterData.AddOnCompletedListener((result) => processResults());
            detailedStatus.AddOnCompletedListener((result) => processResults());

            return encounter;
        }

        protected void ProcessResults(User user,
            WaitableTask<UserEncounter> result,
            WaitableTask<Encounter> encounterData,
            WaitableTask<EncounterStatus> detailedStatus)
        {
            if (result.IsCompleted() || !encounterData.IsCompleted() || !detailedStatus.IsCompleted())
                return;

            var encounter = new UserEncounter(user, encounterData.Result.Value, detailedStatus.Result.Value);
            result.SetResult(encounter);
        }
    }
}
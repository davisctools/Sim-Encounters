namespace ClinicalTools.SimEncounters
{
    public class EncounterReader : IEncounterReader
    {
        private readonly IEncounterDataReaderSelector dataReaderSelector;
        public EncounterReader(IEncounterDataReaderSelector dataReaderSelector)
        {
            this.dataReaderSelector = dataReaderSelector;
        }

        public virtual WaitableTask<Encounter> GetEncounter(User user, EncounterMetadata metadata, SaveType saveType)
        {
            var dataReader = dataReaderSelector.GetEncounterDataReader(saveType);

            var data = dataReader.GetEncounterData(user, metadata);

            var encounterData = new WaitableTask<Encounter>();
            data.AddOnCompletedListener((result) => ProcessResults(encounterData, metadata, result));

            return encounterData;
        }

        protected virtual void ProcessResults(WaitableTask<Encounter> result,
            EncounterMetadata metadata,
            TaskResult<EncounterContent> data)
        {
            if (data.IsError()) {
                result.SetError(data.Exception);
                return;
            }
            var encounterData = new Encounter(metadata, data.Value);
            result.SetResult(encounterData);
        }
    }
}
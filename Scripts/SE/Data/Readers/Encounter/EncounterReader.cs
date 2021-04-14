namespace ClinicalTools.SimEncounters
{
    public class EncounterReader : IEncounterReader
    {
        private readonly IEncounterDataReaderSelector dataReaderSelector;
        public EncounterReader(IEncounterDataReaderSelector dataReaderSelector)
        {
            this.dataReaderSelector = dataReaderSelector;
        }

        public virtual WaitableTask<ContentEncounter> GetEncounter(User user, OldEncounterMetadata metadata, SaveType saveType)
        {
            var dataReader = dataReaderSelector.GetEncounterDataReader(saveType);

            var data = dataReader.GetEncounterData(user, metadata);

            var encounterData = new WaitableTask<ContentEncounter>();
            data.AddOnCompletedListener((result) => ProcessResults(encounterData, metadata, result));

            return encounterData;
        }

        protected virtual void ProcessResults(WaitableTask<ContentEncounter> result,
            OldEncounterMetadata metadata,
            TaskResult<EncounterContentData> data)
        {
            if (data.IsError()) {
                result.SetError(data.Exception);
                return;
            }
            var encounterData = new ContentEncounter(metadata, data.Value);
            result.SetResult(encounterData);
        }
    }
}
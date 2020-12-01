using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public class BasicStatusesReader : IBasicStatusesReader
    {
        private readonly List<IBasicStatusesReader> basicStatusesReaders;
        public BasicStatusesReader(List<IBasicStatusesReader> basicStatusesReaders)
        {
            this.basicStatusesReaders = basicStatusesReaders;
        }

        public WaitableTask<Dictionary<int, EncounterBasicStatus>> GetBasicStatuses(User user)
        {
            var statuses = new WaitableTask<Dictionary<int, EncounterBasicStatus>>();

            var results = new List<WaitableTask<Dictionary<int, EncounterBasicStatus>>>();
            foreach (var basicStatusesReader in basicStatusesReaders)
                results.Add(basicStatusesReader.GetBasicStatuses(user));

            foreach (var result in results)
                result.AddOnCompletedListener((statusesResult) => ProcessResults(statuses, results));

            return statuses;
        }

        private void ProcessResults(WaitableTask<Dictionary<int, EncounterBasicStatus>> result,
            List<WaitableTask<Dictionary<int, EncounterBasicStatus>>> listResults)
        {
            if (result.IsCompleted())
                return;
            foreach (var statusesResult in listResults) {
                if (!statusesResult.IsCompleted())
                    return;
            }

            var statuses = new Dictionary<int, EncounterBasicStatus>();;
            for (int i = 0; i < listResults.Count; i++) { 
                if (listResults[i].Result.HasValue())
                    statuses = CombineStatuses(statuses, listResults[i].Result.Value);
            }
            result.SetResult(statuses);
        }

        private Dictionary<int, EncounterBasicStatus> CombineStatuses(
            Dictionary<int, EncounterBasicStatus> statuses1, Dictionary<int, EncounterBasicStatus> statuses2)
        {
            if (statuses1 == null)
                return statuses2;
            else if (statuses2 == null)
                return statuses1;

            foreach (var status in statuses2) {
                if (!statuses1.ContainsKey(status.Key)) {
                    statuses1.Add(status.Key, status.Value);
                } else if (statuses1[status.Key].Timestamp < status.Value.Timestamp){
                    statuses1[status.Key] = status.Value;
                }
            }
            return statuses1;
        }
    }
}
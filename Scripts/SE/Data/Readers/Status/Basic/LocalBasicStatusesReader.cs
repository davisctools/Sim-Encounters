using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class LocalBasicStatusesReader : IBasicStatusesReader
    {
        private readonly IFileManager fileManager;
        private readonly IStringDeserializer<KeyValuePair<int, EncounterBasicStatus>> parser;
        public LocalBasicStatusesReader(IFileManager fileManager, IStringDeserializer<KeyValuePair<int, EncounterBasicStatus>> parser)
        {
            this.fileManager = fileManager;
            this.parser = parser;
        }

        public WaitableTask<Dictionary<int, EncounterBasicStatus>> GetBasicStatuses(User user)
        {
            var statuses = new WaitableTask<Dictionary<int, EncounterBasicStatus>>();

            var fileTexts = fileManager.GetFilesText(user, FileType.BasicStatus);
            fileTexts.AddOnCompletedListener((result) => ProcessResults(statuses, result));

            return statuses;
        }

        private void ProcessResults(WaitableTask<Dictionary<int, EncounterBasicStatus>> result, TaskResult<string[]> fileTexts)
        {
            if (!fileTexts.HasValue()) {
                result.SetResult(new Dictionary<int, EncounterBasicStatus>());
                return;
            }

            var statuses = new Dictionary<int, EncounterBasicStatus>();
            foreach (var fileText in fileTexts.Value) {
                var metadata = parser.Deserialize(fileText);
                if (statuses.ContainsKey(metadata.Key)) {
                    Debug.LogError($"Duplicate saved status for key {metadata.Key}");
                    continue;
                }
                if (metadata.Value != null)
                    statuses.Add(metadata.Key, metadata.Value);
            }

            result.SetResult(statuses);
        }
    }
}
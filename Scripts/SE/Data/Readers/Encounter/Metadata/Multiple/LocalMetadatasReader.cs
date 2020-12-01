
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public class LocalMetadatasReader : IMetadatasReader
    {
        private readonly IFileManager fileManager;
        private readonly IStringDeserializer<EncounterMetadata> parser;
        public LocalMetadatasReader(IFileManager fileManager, IStringDeserializer<EncounterMetadata> parser)
        {
            this.fileManager = fileManager;
            this.parser = parser;
        }

        public WaitableTask<List<EncounterMetadata>> GetMetadatas(User user)
        {
            var metadatas = new WaitableTask<List<EncounterMetadata>>();

            var fileTexts = fileManager.GetFilesText(user, FileType.Metadata);
            fileTexts.AddOnCompletedListener((result) => ProcessResults(metadatas, result));

            return metadatas;
        }

        private void ProcessResults(WaitableTask<List<EncounterMetadata>> result, TaskResult<string[]> fileTexts)
        {
            if (fileTexts == null) {
                result.SetError(null);
                return;
            }

            var metadatas = new List<EncounterMetadata>();
            foreach (var fileText in fileTexts.Value) {
                var metadata = parser.Deserialize(fileText);
                if (metadata != null)
                    metadatas.Add(metadata);
            }

            result.SetResult(metadatas);
        }
    }
}

using SimpleJSON;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public class LocalEncountersMetadataJsonRetriever : IEncountersMetadataJsonRetriever
    {
        protected IEncounterFileReader FileReader { get; }
        public LocalEncountersMetadataJsonRetriever(IEncounterFileReader fileReader) => FileReader = fileReader;

        public WaitableTask<IEnumerable<JSONNode>> GetMetadataJsonNodes(User user)
        {
            var metadataFilesTextTask = FileReader.ReadTextFiles(user, EncounterDataFileType.Metadata);

            var metadataNodes = new WaitableTask<IEnumerable<JSONNode>>();
            metadataFilesTextTask.AddOnCompletedListener((result) => ProcessResults(metadataNodes, result, user));

            return metadataNodes;
        }

        protected virtual void ProcessResults(WaitableTask<IEnumerable<JSONNode>> task, TaskResult<string[]> metadataFilesText, User user)
        {
            if (metadataFilesText == null || metadataFilesText.IsError()) {
                task.SetError(metadataFilesText.Exception);
                return;
            }

            var nodesList = new List<JSONNode>();
            foreach (var metadataFileText in metadataFilesText.Value)
                nodesList.Add(JSON.Parse(metadataFileText));

            task.SetResult(nodesList);
        }
    }
}

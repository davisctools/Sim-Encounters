namespace ClinicalTools.SimEncounters
{
    public class LocalMetadataReader : IMetadataReader
    {
        private readonly IFileManager fileManager;
        private readonly IStringDeserializer<OldEncounterMetadata> parser;
        public LocalMetadataReader(IFileManager fileManager, IStringDeserializer<OldEncounterMetadata> parser)
        {
            this.fileManager = fileManager;
            this.parser = parser;
        }

        public WaitableTask<OldEncounterMetadata> GetMetadata(User user, OldEncounterMetadata metadata)
        {
            var metadataResult = new WaitableTask<OldEncounterMetadata>();

            var fileText = fileManager.GetFileText(user, FileType.Metadata, metadata);
            fileText.AddOnCompletedListener((result) => ProcessResults(metadataResult, result));

            return metadataResult;
        }

        private void ProcessResults(WaitableTask<OldEncounterMetadata> result, TaskResult<string> fileText)
        {
            if (fileText.Value == null) {
                result.SetError(null);
                return;
            }

            var metadata = parser.Deserialize(fileText.Value);
            result.SetResult(metadata);
        }
    }
}

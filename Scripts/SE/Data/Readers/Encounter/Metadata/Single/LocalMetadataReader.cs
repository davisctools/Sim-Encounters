namespace ClinicalTools.SimEncounters
{
    public class LocalMetadataReader : IMetadataReader
    {
        private readonly IFileManager fileManager;
        private readonly IStringDeserializer<EncounterMetadata> parser;
        public LocalMetadataReader(IFileManager fileManager, IStringDeserializer<EncounterMetadata> parser)
        {
            this.fileManager = fileManager;
            this.parser = parser;
        }

        public WaitableTask<EncounterMetadata> GetMetadata(User user, EncounterMetadata metadata)
        {
            var metadataResult = new WaitableTask<EncounterMetadata>();

            var fileText = fileManager.GetFileText(user, FileType.Metadata, metadata);
            fileText.AddOnCompletedListener((result) => ProcessResults(metadataResult, result));

            return metadataResult;
        }

        private void ProcessResults(WaitableTask<EncounterMetadata> result, TaskResult<string> fileText)
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

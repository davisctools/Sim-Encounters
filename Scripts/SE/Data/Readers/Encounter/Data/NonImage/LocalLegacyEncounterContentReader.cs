namespace ClinicalTools.SimEncounters
{
    public class LocalLegacyEncounterContentReader : IEncounterDataReader
    {
        private readonly IFileManager fileManager;
        private readonly IStringDeserializer<EncounterContentData> parser;
        public LocalLegacyEncounterContentReader(IFileManager fileManager, IStringDeserializer<EncounterContentData> parser)
        {
            this.fileManager = fileManager;
            this.parser = parser;
        }

        public WaitableTask<EncounterContentData> GetEncounterData(User user, OldEncounterMetadata metadata)
        {
            var content = new WaitableTask<EncounterContentData>();

            var fileText = fileManager.GetFileText(user, FileType.Data, metadata);
            fileText.AddOnCompletedListener((result) => ProcessResults(content, result));

            return content;
        }

        private void ProcessResults(WaitableTask<EncounterContentData> result, TaskResult<string> fileText)
        {
            if (fileText.IsError())
                result.SetError(fileText.Exception);
            else
                result.SetResult(parser.Deserialize(fileText.Value));
        }
    }
}
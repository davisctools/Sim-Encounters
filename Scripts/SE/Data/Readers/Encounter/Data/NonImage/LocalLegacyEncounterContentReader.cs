namespace ClinicalTools.SimEncounters
{
    public class LocalLegacyEncounterContentReader : IEncounterDataReader
    {
        private readonly IFileManager fileManager;
        private readonly IStringDeserializer<EncounterContent> parser;
        public LocalLegacyEncounterContentReader(IFileManager fileManager, IStringDeserializer<EncounterContent> parser)
        {
            this.fileManager = fileManager;
            this.parser = parser;
        }

        public WaitableTask<EncounterContent> GetEncounterData(User user, EncounterMetadata metadata)
        {
            var content = new WaitableTask<EncounterContent>();

            var fileText = fileManager.GetFileText(user, FileType.Data, metadata);
            fileText.AddOnCompletedListener((result) => ProcessResults(content, result));

            return content;
        }

        private void ProcessResults(WaitableTask<EncounterContent> result, TaskResult<string> fileText)
        {
            if (fileText.IsError())
                result.SetError(fileText.Exception);
            else
                result.SetResult(parser.Deserialize(fileText.Value));
        }
    }
}
namespace ClinicalTools.SimEncounters
{
    public class LocalNonImageContentReader : INonImageContentReader
    {
        private readonly IFileManager fileManager;
        private readonly IStringDeserializer<EncounterNonImageContent> parser;
        public LocalNonImageContentReader(IFileManager fileManager, IStringDeserializer<EncounterNonImageContent> parser)
        {
            this.fileManager = fileManager;
            this.parser = parser;
        }

        public WaitableTask<EncounterNonImageContent> GetNonImageContent(User user, EncounterMetadata metadata)
        {
            var content = new WaitableTask<EncounterNonImageContent>();

            var fileText = fileManager.GetFileText(user, FileType.Data, metadata);
            fileText.AddOnCompletedListener((result) => ProcessResults(content, result));

            return content;
        }

        private void ProcessResults(WaitableTask<EncounterNonImageContent> result, TaskResult<string> fileText)
        {
            if (fileText.IsError())
                result.SetError(fileText.Exception);
            else
                result.SetResult(parser.Deserialize(fileText.Value));
        }
    }
}
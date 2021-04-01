namespace ClinicalTools.SimEncounters
{
    public class LocalImageContentReader : IImageContentReader
    {
        private readonly IFileManager fileManager;
        private readonly IStringDeserializer<LegacyEncounterImageContent> parser;
        public LocalImageContentReader(IFileManager fileManager, IStringDeserializer<LegacyEncounterImageContent> parser)
        {
            this.fileManager = fileManager;
            this.parser = parser;
        }

        public WaitableTask<LegacyEncounterImageContent> GetImageData(User user, EncounterMetadata metadata)
        {
            var imageData = new WaitableTask<LegacyEncounterImageContent>();

            var fileText = fileManager.GetFileText(user, FileType.Image, metadata);
            fileText.AddOnCompletedListener((result) => ProcessResults(imageData, result));

            return imageData;
        }

        private void ProcessResults(WaitableTask<LegacyEncounterImageContent> result, TaskResult<string> fileText)
        {
            if (fileText.IsError())
                result.SetError(fileText.Exception);
            else
                result.SetResult(parser.Deserialize(fileText.Value));
        }
    }
}
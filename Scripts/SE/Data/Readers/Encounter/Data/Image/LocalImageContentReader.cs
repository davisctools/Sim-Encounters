namespace ClinicalTools.SimEncounters
{
    public class LocalImageContentReader : IImageContentReader
    {
        private readonly IFileManager fileManager;
        private readonly IStringDeserializer<EncounterImageContent> parser;
        public LocalImageContentReader(IFileManager fileManager, IStringDeserializer<EncounterImageContent> parser)
        {
            this.fileManager = fileManager;
            this.parser = parser;
        }

        public WaitableTask<EncounterImageContent> GetImageData(User user, EncounterMetadata metadata)
        {
            var imageData = new WaitableTask<EncounterImageContent>();

            var fileText = fileManager.GetFileText(user, FileType.Image, metadata);
            fileText.AddOnCompletedListener((result) => ProcessResults(imageData, result));

            return imageData;
        }

        private void ProcessResults(WaitableTask<EncounterImageContent> result, TaskResult<string> fileText)
        {
            if (fileText.IsError())
                result.SetError(fileText.Exception);
            else
                result.SetResult(parser.Deserialize(fileText.Value));
        }
    }
}
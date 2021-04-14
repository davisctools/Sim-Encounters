namespace ClinicalTools.SimEncounters
{
    public class LegacyEncounterDataReader : IEncounterDataReader
    {
        protected IEncounterDataReader ContentReader { get; }
        protected IImageContentReader ImageDataReader { get; }
        public LegacyEncounterDataReader(IEncounterDataReader contentReader, IImageContentReader imageDataReader)
        {
            ContentReader = contentReader;
            ImageDataReader = imageDataReader;
        }

        public virtual WaitableTask<EncounterContentData> GetEncounterData(User user, OldEncounterMetadata metadata)
        {
            var encounterData = new WaitableTask<EncounterContentData>();
            var content = ContentReader.GetEncounterData(user, metadata);
            var imageData = ImageDataReader.GetImageData(user, metadata);

            content.AddOnCompletedListener((result) => ProcessResults(encounterData, content, imageData));
            imageData.AddOnCompletedListener((result) => ProcessResults(encounterData, content, imageData));

            return encounterData;
        }

        protected virtual void ProcessResults(WaitableTask<EncounterContentData> result,
            WaitableTask<EncounterContentData> content,
            WaitableTask<LegacyEncounterImageContent> imageData)
        {
            if (result.IsCompleted() || !content.IsCompleted() || !imageData.IsCompleted())
                return;

            // TODO: add images
            //var encounterData = new OldEncounterContent(content.Result.Value, imageData.Result.Value);
            result.SetResult(content.Result.Value);
        }
    }
}
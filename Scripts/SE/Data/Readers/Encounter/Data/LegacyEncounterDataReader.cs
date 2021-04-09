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

        public virtual WaitableTask<EncounterContent> GetEncounterData(User user, EncounterMetadata metadata)
        {
            var encounterData = new WaitableTask<EncounterContent>();
            var content = ContentReader.GetEncounterData(user, metadata);
            var imageData = ImageDataReader.GetImageData(user, metadata);

            content.AddOnCompletedListener((result) => ProcessResults(encounterData, content, imageData));
            imageData.AddOnCompletedListener((result) => ProcessResults(encounterData, content, imageData));

            return encounterData;
        }

        protected virtual void ProcessResults(WaitableTask<EncounterContent> result,
            WaitableTask<EncounterContent> content,
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
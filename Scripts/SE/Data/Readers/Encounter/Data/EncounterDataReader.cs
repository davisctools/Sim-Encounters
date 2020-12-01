namespace ClinicalTools.SimEncounters
{
    public class EncounterDataReader : IEncounterDataReader
    {
        protected INonImageContentReader ContentReader { get; }
        protected IImageContentReader ImageDataReader { get; }
        public EncounterDataReader(INonImageContentReader contentReader, IImageContentReader imageDataReader)
        {
            ContentReader = contentReader;
            ImageDataReader = imageDataReader;
        }

        public virtual WaitableTask<EncounterContent> GetEncounterData(User user, EncounterMetadata metadata)
        {
            var encounterData = new WaitableTask<EncounterContent>();
            var content = ContentReader.GetNonImageContent(user, metadata);
            var imageData = ImageDataReader.GetImageData(user, metadata);

            content.AddOnCompletedListener((result) => ProcessResults(encounterData, content, imageData));
            imageData.AddOnCompletedListener((result) => ProcessResults(encounterData, content, imageData));

            return encounterData;
        }

        protected virtual void ProcessResults(WaitableTask<EncounterContent> result,
            WaitableTask<EncounterNonImageContent> content,
            WaitableTask<EncounterImageContent> imageData)
        {
            if (result.IsCompleted() || !content.IsCompleted() || !imageData.IsCompleted())
                return;

            var encounterData = new EncounterContent(content.Result.Value, imageData.Result.Value);
            result.SetResult(encounterData);
        }
    }
}
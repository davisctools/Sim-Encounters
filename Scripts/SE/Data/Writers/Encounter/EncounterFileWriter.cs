using System.Xml;

namespace ClinicalTools.SimEncounters
{
    public class EncounterFileWriter : IEncounterWriter
    {
        protected IMetadataWriter MetadataWriter { get; }
        protected IFileManager FileManager { get; }
        protected IObjectSerializer<LegacyEncounterImageContent> ImageDataSerializer { get; }
        protected IObjectSerializer<EncounterContentData> EncounterContentSerializer { get; }
        public EncounterFileWriter(
            IMetadataWriter metadataWriter,
            IFileManager fileManager, 
            IObjectSerializer<LegacyEncounterImageContent> imageDataSerializer, 
            IObjectSerializer<EncounterContentData> encounterContentSerializer)
        {
            MetadataWriter = metadataWriter;
            FileManager = fileManager;
            ImageDataSerializer = imageDataSerializer;
            EncounterContentSerializer = encounterContentSerializer;
        }

        public WaitableTask Save(SaveEncounterParameters parameters)
        {
            MetadataWriter.Save(parameters.User, parameters.Encounter.Metadata);

            var contentDoc = new XmlDocument();
            var contentSerializer = new XmlSerializer(contentDoc);
            EncounterContentSerializer.Serialize(contentSerializer, parameters.Encounter.Content);
            FileManager.SetFileText(parameters.User, FileType.Data, parameters.Encounter.Metadata, contentDoc.OuterXml);

            var imagesDoc = new XmlDocument();
            var imagesSerializer = new XmlSerializer(imagesDoc);
            //ImageDataSerializer.Serialize(imagesSerializer, encounter.Content.ImageContent);
            //FileManager.SetFileText(user, FileType.Image, encounter.Metadata, imagesDoc.OuterXml);

            return WaitableTask.CompletedTask;
        }
    }
}

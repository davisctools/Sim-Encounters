using System.Xml;

namespace ClinicalTools.SimEncounters
{
    public class EncounterFileWriter : IEncounterWriter
    {
        protected IMetadataWriter MetadataWriter { get; }
        protected IFileManager FileManager { get; }
        protected IXmlSerializer<EncounterImageContent> ImageDataSerializer { get; }
        protected IXmlSerializer<EncounterNonImageContent> EncounterContentSerializer { get; }
        public EncounterFileWriter(
            IMetadataWriter metadataWriter,
            IFileManager fileManager, 
            IXmlSerializer<EncounterImageContent> imageDataSerializer, 
            IXmlSerializer<EncounterNonImageContent> encounterContentSerializer)
        {
            MetadataWriter = metadataWriter;
            FileManager = fileManager;
            ImageDataSerializer = imageDataSerializer;
            EncounterContentSerializer = encounterContentSerializer;
        }

        public WaitableTask Save(User user, Encounter encounter)
        {
            MetadataWriter.Save(user, encounter.Metadata);

            var contentDoc = new XmlDocument();
            var contentSerializer = new XmlSerializer(contentDoc);
            EncounterContentSerializer.Serialize(contentSerializer, encounter.Content.NonImageContent);
            FileManager.SetFileText(user, FileType.Data, encounter.Metadata, contentDoc.OuterXml);

            var imagesDoc = new XmlDocument();
            var imagesSerializer = new XmlSerializer(imagesDoc);
            ImageDataSerializer.Serialize(imagesSerializer, encounter.Content.ImageContent);
            FileManager.SetFileText(user, FileType.Image, encounter.Metadata, imagesDoc.OuterXml);

            return WaitableTask.CompletedTask;
        }
    }
}

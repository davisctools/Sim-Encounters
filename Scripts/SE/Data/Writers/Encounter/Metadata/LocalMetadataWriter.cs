namespace ClinicalTools.SimEncounters
{
    public class LocalMetadataWriter : IMetadataWriter
    {
        protected IFileManager FileManager { get; }
        protected IStringSerializer<EncounterMetadata> MetadataSerializer { get; }
        public LocalMetadataWriter(IFileManager fileManager, IStringSerializer<EncounterMetadata> metadataSerializer)
        {
            FileManager = fileManager;
            MetadataSerializer = metadataSerializer;
        }

        public void Save(User user, EncounterMetadata metadata)
        {
            metadata.ResetDateModified();
            var metadataText = MetadataSerializer.Serialize(metadata);
            FileManager.SetFileText(user, FileType.Metadata, metadata, metadataText);
        }
    }
}

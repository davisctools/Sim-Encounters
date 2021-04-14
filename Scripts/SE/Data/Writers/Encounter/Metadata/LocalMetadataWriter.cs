namespace ClinicalTools.SimEncounters
{
    public class LocalMetadataWriter : IMetadataWriter
    {
        protected IFileManager FileManager { get; }
        protected IStringSerializer<OldEncounterMetadata> MetadataSerializer { get; }
        public LocalMetadataWriter(IFileManager fileManager, IStringSerializer<OldEncounterMetadata> metadataSerializer)
        {
            FileManager = fileManager;
            MetadataSerializer = metadataSerializer;
        }

        public void Save(User user, OldEncounterMetadata metadata)
        {
            metadata.ResetDateModified();
            var metadataText = MetadataSerializer.Serialize(metadata);
            FileManager.SetFileText(user, FileType.Metadata, metadata, metadataText);
        }
    }
}

namespace ClinicalTools.SimEncounters
{
    public class FilenameInfo : IFilenameInfo
    {
        public virtual string ImagesFolderName { get; } = "images";
        protected virtual string ImagesFileName { get; } = "images.json";
        protected virtual string DataFileName { get; } = "data.xml";
        protected virtual string MetadataFileName { get; } = "metadata.json";

        public string GetFilename(EncounterDataFileType fileType)
        {
            switch (fileType) {
                case EncounterDataFileType.Data:
                    return DataFileName;
                case EncounterDataFileType.Images:
                    return ImagesFileName;
                case EncounterDataFileType.Metadata:
                    return MetadataFileName;
                default:
                    return null;
            }
        }
    }
}
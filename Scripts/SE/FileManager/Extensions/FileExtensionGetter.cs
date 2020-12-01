namespace ClinicalTools.SimEncounters
{
    public class FileExtensionGetter : IFileExtensionGetter
    {
        public string GetExtension(FileType fileType)
        {
            switch (fileType)
            {
                case FileType.Data:
                    return "ced";
                case FileType.Image:
                    return "cei";
                case FileType.BasicStatus:
                    return "ces";
                case FileType.DetailedStatus:
                    return "cesd";
                case FileType.Metadata:
                    return "cem";
                default:
                    return null;
            }
        }
    }
}
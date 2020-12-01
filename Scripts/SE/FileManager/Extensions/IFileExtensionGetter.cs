namespace ClinicalTools.SimEncounters
{
    public interface IFileExtensionGetter
    {
        string GetExtension(FileType fileType);
    }
}
namespace ClinicalTools.SimEncounters
{
    public interface IFilenameGetter
    {
        string GetFilename(FileType fileType, EncounterMetadata metadata);
    }
}
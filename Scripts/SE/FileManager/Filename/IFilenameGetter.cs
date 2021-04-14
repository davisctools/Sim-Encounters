namespace ClinicalTools.SimEncounters
{
    public interface IFilenameGetter
    {
        string GetFilename(FileType fileType, OldEncounterMetadata metadata);
    }
}
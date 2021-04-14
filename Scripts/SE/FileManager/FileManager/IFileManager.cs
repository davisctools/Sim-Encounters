namespace ClinicalTools.SimEncounters
{
    public interface IFileManager
    {
        void SetFileText(User user, FileType fileType, OldEncounterMetadata metadata, string contents);

        WaitableTask<string> GetFileText(User user, FileType fileType, OldEncounterMetadata metadata);
        WaitableTask<string[]> GetFilesText(User user, FileType fileType);

        void UpdateFilename(User user, OldEncounterMetadata metadata);
        void DeleteFiles(User user, OldEncounterMetadata metadata);
    }
}
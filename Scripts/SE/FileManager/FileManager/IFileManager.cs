namespace ClinicalTools.SimEncounters
{
    public interface IFileManager
    {
        void SetFileText(User user, FileType fileType, EncounterMetadata metadata, string contents);

        WaitableTask<string> GetFileText(User user, FileType fileType, EncounterMetadata metadata);
        WaitableTask<string[]> GetFilesText(User user, FileType fileType);

        void UpdateFilename(User user, EncounterMetadata metadata);
        void DeleteFiles(User user, EncounterMetadata metadata);
    }
}
namespace ClinicalTools.SimEncounters
{
    public interface IEncounterFileWriter
    {
        void WriteTextFile(User user, OldEncounterMetadata metadata, EncounterDataFileType fileType, string contents);
        void WriteTextureFile(User user, OldEncounterMetadata metadata, EncounterImage image);
        void DeleteFiles(User user, OldEncounterMetadata metadata);
    }
}
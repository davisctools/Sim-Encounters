namespace ClinicalTools.SimEncounters
{
    public interface IEncounterFileWriter
    {
        void WriteTextFile(User user, EncounterMetadata metadata, EncounterDataFileType fileType, string contents);
        void WriteTextureFile(User user, EncounterMetadata metadata, EncounterImage image);
        void DeleteFiles(User user, EncounterMetadata metadata);
    }
}
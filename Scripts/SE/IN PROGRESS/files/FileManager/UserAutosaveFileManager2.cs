using System.IO;

namespace ClinicalTools.SimEncounters
{
    public class UserAutosaveFileManager2 : UserFileManager2
    {
        public UserAutosaveFileManager2(IFilenameInfo filenameInfo) : base(filenameInfo) { }
        protected override string SaveFolder { get; set; } = "autosave";

        public override void DeleteFiles(User user, OldEncounterMetadata metadata)
        {
            var encounterFolder = GetSaveFolder(user, metadata);
            Directory.Delete(encounterFolder, true);
        }
    }
}
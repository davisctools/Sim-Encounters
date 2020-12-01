using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class LocalEncounterWriter : IEncounterWriter
    {
        protected IEncounterWriter MainDataWriter { get; }
        protected IFileManager LocalFileManager { get; }
        protected IFileManager AutosaveFileManager { get; }
        public LocalEncounterWriter(
            IEncounterWriter localEncounterSaver, 
            [Inject(Id = SaveType.Local)] IFileManager localFileManager, 
            [Inject(Id = SaveType.Autosave)] IFileManager autosaveFileManager)
        {
            MainDataWriter = localEncounterSaver;
            LocalFileManager = localFileManager;
            AutosaveFileManager = autosaveFileManager;
        }

        public WaitableTask Save(User user, Encounter encounter)
        {
            AutosaveFileManager.DeleteFiles(user, encounter.Metadata);
            LocalFileManager.UpdateFilename(user, encounter.Metadata);
            return MainDataWriter.Save(user, encounter);
        }
    }
}
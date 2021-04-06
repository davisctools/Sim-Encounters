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

        public WaitableTask Save(SaveEncounterParameters parameters)
        {
            AutosaveFileManager.DeleteFiles(parameters.User, parameters.Encounter.Metadata);
            LocalFileManager.UpdateFilename(parameters.User, parameters.Encounter.Metadata);
            return MainDataWriter.Save(parameters);
        }
    }
}
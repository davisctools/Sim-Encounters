namespace ClinicalTools.SimEncounters
{
    public class LocalDetailedStatusReader : IDetailedStatusReader
    {
        private readonly IFileManager fileManager;
        private readonly IStringDeserializer<EncounterContentStatus> parser;
        public LocalDetailedStatusReader(IFileManager fileManager, IStringDeserializer<EncounterContentStatus> parser)
        {
            this.fileManager = fileManager;
            this.parser = parser;
        }

        public WaitableTask<EncounterStatus> GetDetailedStatus(User user,
            OldEncounterMetadata metadata, EncounterBasicStatus basicStatus)
        {
            var detailedStatus = new WaitableTask<EncounterStatus>();

            var fileText = fileManager.GetFileText(user, FileType.DetailedStatus, metadata);
            fileText.AddOnCompletedListener((result) => ProcessResults(detailedStatus, result, basicStatus));

            return detailedStatus;
        }

        private void ProcessResults(WaitableTask<EncounterStatus> result, 
            TaskResult<string> fileText, EncounterBasicStatus basicStatus)
        {
            EncounterContentStatus detailedStatus = (fileText.IsError()) ? new EncounterContentStatus() : parser.Deserialize(fileText.Value);
            var status = new EncounterStatus(basicStatus, detailedStatus);
            result.SetResult(status);
        }
    }
}
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public class LocalStatusWriter : IStatusWriter
    {
        protected IFileManager FileManager { get; }
        protected IStringSerializer<EncounterContentStatus> ContentStatusSerializer { get; }
        protected IStringSerializer<KeyValuePair<int, EncounterBasicStatus>> BasicStatusSerializer { get; }
        public LocalStatusWriter(
            IFileManager fileManager,
            IStringSerializer<EncounterContentStatus> contentStatusSerializer,
            IStringSerializer<KeyValuePair<int, EncounterBasicStatus>> basicStatusSerializer)
        {
            FileManager = fileManager;
            ContentStatusSerializer = contentStatusSerializer;
            BasicStatusSerializer = basicStatusSerializer;
        }

        public WaitableTask WriteStatus(UserEncounter encounter)
        {
            var metadata = encounter.Data.Metadata;
            var status = encounter.Status;

            var statusKeyValuePair = new KeyValuePair<int, EncounterBasicStatus>(metadata.RecordNumber, status.BasicStatus);
            var basicStatusString = BasicStatusSerializer.Serialize(statusKeyValuePair);
            FileManager.SetFileText(encounter.User, FileType.BasicStatus, metadata, basicStatusString);

            var statusString = ContentStatusSerializer.Serialize(status.ContentStatus);
            FileManager.SetFileText(encounter.User, FileType.DetailedStatus, metadata, statusString);

            return WaitableTask.CompletedTask;
        }
    }
}
namespace ClinicalTools.SimEncounters
{
    public class EncounterStatus
    {
        public EncounterBasicStatus BasicStatus { get; }
        public EncounterContentStatus ContentStatus { get; }
        public long Timestamp { get; set; }

        public EncounterStatus(EncounterBasicStatus basicStatus, EncounterContentStatus contentStatus) {
            if (basicStatus == null)
                basicStatus = new EncounterBasicStatus();
            if (contentStatus == null)
                contentStatus = new EncounterContentStatus();

            BasicStatus = basicStatus;
            ContentStatus = contentStatus;
        }
    }
}
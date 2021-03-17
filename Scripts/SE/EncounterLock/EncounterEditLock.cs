namespace ClinicalTools.SimEncounters
{
    public class EncounterEditLock
    {
        public int RecordNumber { get; set; }
        public string EditorName { get; set; }
        public long StartEditTime { get; set; }
    }
}

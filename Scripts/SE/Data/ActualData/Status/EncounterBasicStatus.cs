namespace ClinicalTools.SimEncounters
{
    /// <summary>
    /// Encounter status information needed by the main menu
    /// </summary>
    public class EncounterBasicStatus
    {
        public bool Completed { get; set; }
        public int Rating { get; set; }
        public long Timestamp { get; set; }
    }
}
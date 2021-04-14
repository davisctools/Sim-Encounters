namespace ClinicalTools.SimEncounters
{
    public class EncounterImmutableMetadata
    {
        public virtual int Id { get; set; }
        public virtual Author Author { get; set; }
        public virtual int CreationDate { get; set; }
    }
}
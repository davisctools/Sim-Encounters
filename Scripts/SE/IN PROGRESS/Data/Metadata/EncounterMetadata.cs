namespace ClinicalTools.SimEncounters
{
    public class EncounterMetadata
    {
        public virtual EncounterImmutableMetadata Immutable { get; }
        public virtual EncounterMutableMetadata Mutable { get; set; } = new EncounterMutableMetadata();

        public EncounterMetadata(EncounterImmutableMetadata immutable) => Immutable = immutable;
    }
}
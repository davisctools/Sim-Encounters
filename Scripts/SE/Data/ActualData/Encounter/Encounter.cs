namespace ClinicalTools.SimEncounters
{
    public class Encounter
    {
        public virtual EncounterMetadata Metadata { get; }
        public virtual EncounterContent Content { get; }

        public Encounter(EncounterMetadata metadata, EncounterContent content)
        {
            Metadata = metadata;
            Content = content;
        }
    }
}
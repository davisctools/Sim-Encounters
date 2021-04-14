namespace ClinicalTools.SimEncounters
{
    public class ContentEncounter
    {
        public virtual OldEncounterMetadata Metadata { get; }
        public virtual EncounterContentData Content { get; }

        public ContentEncounter(OldEncounterMetadata metadata, EncounterContentData content)
        {
            Metadata = metadata;
            Content = content;
        }
    }
}
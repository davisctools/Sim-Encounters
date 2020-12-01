namespace ClinicalTools.SimEncounters
{
    public class EncounterContent
    {
        public EncounterNonImageContent NonImageContent { get; }
        public EncounterImageContent ImageContent { get; }

        public EncounterContent(EncounterNonImageContent content, EncounterImageContent imageData)
        {
            NonImageContent = content;
            ImageContent = imageData;
        }
    }
}
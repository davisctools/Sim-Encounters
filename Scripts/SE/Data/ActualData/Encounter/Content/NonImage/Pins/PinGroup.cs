namespace ClinicalTools.SimEncounters
{
    public class PinGroup
    {
        public virtual ReadMorePin ReadMore { get; set; }
        public virtual DialoguePin Dialogue { get; set; }
        public virtual QuizPin Quiz { get; set; }

        public virtual bool HasPin() => ReadMore != null || Dialogue != null || Quiz != null;
    }
}
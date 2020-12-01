using ClinicalTools.Collections;

namespace ClinicalTools.SimEncounters
{
    public class DialoguePin
    {
        public virtual OrderedCollection<Panel> Conversation { get; set; } = new OrderedCollection<Panel>();
    }
}
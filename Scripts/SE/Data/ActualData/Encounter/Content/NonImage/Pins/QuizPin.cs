using ClinicalTools.Collections;

namespace ClinicalTools.SimEncounters
{
    public class QuizPin
    {
        public virtual OrderedCollection<Panel> Questions { get; set; } = new OrderedCollection<Panel>();
    }
}
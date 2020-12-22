using ClinicalTools.Collections;

namespace ClinicalTools.SimEncounters
{
    public class ReadMorePin
    {
        public virtual OrderedCollection<Panel> Panels { get; set; } = new OrderedCollection<Panel>();
    }
}
using ClinicalTools.Collections;

namespace ClinicalTools.SimEncounters
{
    public class Tab
    {
        public virtual string Type { get; }
        public virtual string Name { get; set; }
        public virtual OrderedCollection<Panel> Panels { get; set; } = new OrderedCollection<Panel>();

        /**
         * Data script to hold info for every tab. Stored in SectionDataScript.Dict
         */
        public Tab(string type, string name)
        {
            Type = type;
            Name = name;
        }
    }
}
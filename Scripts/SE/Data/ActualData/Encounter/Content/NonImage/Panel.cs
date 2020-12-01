using ClinicalTools.Collections;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public class Panel
    {
        public string Type { get; set; }
        public IDictionary<string, string> Values { get; set; } = new Dictionary<string, string>();
        public OrderedCollection<Panel> ChildPanels { get; set; } = new OrderedCollection<Panel>();
        public virtual PinGroup Pins { get; set; }

        public Panel(string type)
        {
            Type = type;
        }
        public Panel(string type, OrderedCollection<Panel> panels)
        {
            Type = type;
            ChildPanels = panels;
        }
    }
}
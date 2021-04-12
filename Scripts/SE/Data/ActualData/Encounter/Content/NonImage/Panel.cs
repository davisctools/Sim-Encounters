using ClinicalTools.Collections;
using ClinicalTools.SEColors;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{

    public class EncounterValueGroup
    {
        public IDictionary<string, string> CharacterKeys { get; set; } = new Dictionary<string, string>();
        public IDictionary<string, string> TextValues { get; set; } = new Dictionary<string, string>();
        public IDictionary<string, string> ImageKeys { get; set; } = new Dictionary<string, string>();
        public IDictionary<string, string> IconKeys { get; set; } = new Dictionary<string, string>();
        public IDictionary<string, Color> ColorValues { get; set; } = new Dictionary<string, Color>();
    }

    public class Panel
    {
        public string Type { get; set; }
        // Legacy
        public IDictionary<string, string> LegacyValues { get; set; } = new Dictionary<string, string>();

        public EncounterValueGroup Values { get; set; } = new EncounterValueGroup();


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
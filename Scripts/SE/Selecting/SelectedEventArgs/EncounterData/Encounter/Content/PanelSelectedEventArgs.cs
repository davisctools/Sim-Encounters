using System;

namespace ClinicalTools.SimEncounters
{
    public class PanelSelectedEventArgs : EventArgs
    {
        public Panel Panel { get; }
        public PanelSelectedEventArgs(Panel panel) => Panel = panel;
    }
}
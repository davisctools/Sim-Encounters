using ClinicalTools.Collections;
using System;

namespace ClinicalTools.SimEncounters
{
    public class ChildUserPanelsSelectedEventArgs : EventArgs
    {
        public OrderedCollection<UserPanel> ChildPanels { get; }
        public bool Active { get; }
        public ChildUserPanelsSelectedEventArgs(OrderedCollection<UserPanel> childPanels, bool active)
        {
            ChildPanels = childPanels;
            Active = active;
        }
    }
}
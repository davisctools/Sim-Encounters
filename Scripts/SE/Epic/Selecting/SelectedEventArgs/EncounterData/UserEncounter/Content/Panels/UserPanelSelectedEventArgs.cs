using System;

namespace ClinicalTools.SimEncounters
{
    public class UserPanelSelectedEventArgs : EventArgs
    {
        public UserPanel SelectedPanel { get; }
        public bool Active { get; }
        public UserPanelSelectedEventArgs(UserPanel selectedPanel, bool active)
        {
            SelectedPanel = selectedPanel;
            Active = active;
        }
    }
}
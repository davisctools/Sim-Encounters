using System;

namespace ClinicalTools.SimEncounters
{
    public delegate void TabSelectedHandler(object sender, TabSelectedEventArgs e);
    public class TabSelectedEventArgs : EventArgs
    {
        public Tab SelectedTab { get; }
        public TabSelectedEventArgs(Tab selectedTab)
        {
            SelectedTab = selectedTab;
        }
    }
}
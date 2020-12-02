using System;

namespace ClinicalTools.SimEncounters
{
    public class UserSectionSelectedEventArgs : EventArgs
    {
        public UserSection SelectedSection { get; }
        public ChangeType ChangeType { get; }
        public UserSectionSelectedEventArgs(UserSection selectedSection, ChangeType changeType)
        {
            SelectedSection = selectedSection;
            ChangeType = changeType;
        }
    }
}
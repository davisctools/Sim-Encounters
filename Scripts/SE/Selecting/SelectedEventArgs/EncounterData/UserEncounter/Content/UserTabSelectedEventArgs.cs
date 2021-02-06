using System;

namespace ClinicalTools.SimEncounters
{
    public enum ChangeType
    {
        Inactive, Previous, Next, JumpTo, MoveTo
    }

    public class UserTabSelectedEventArgs : EventArgs
    {
        public UserTab SelectedTab { get; }
        public ChangeType ChangeType { get; }
        public UserTabSelectedEventArgs(UserTab selectedTab, ChangeType changeType)
        {
            SelectedTab = selectedTab;
            ChangeType = changeType;
        }

        public override bool Equals(object obj)
        {
            if (obj is UserTabSelectedEventArgs other)
                return SelectedTab == other.SelectedTab && ChangeType == other.ChangeType;

            return base.Equals(obj);
        }

        public override int GetHashCode() => base.GetHashCode();
        public override string ToString() => base.ToString();
    }
}
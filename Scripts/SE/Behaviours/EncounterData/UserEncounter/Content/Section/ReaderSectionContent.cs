using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ReaderSectionContent : UserSectionSelectorBehaviour
    {
        public RectTransform RectTransform => (RectTransform)transform;
        public UserSection Section => UserSectionValue?.SelectedSection;
        public UserTab Tab => UserTabValue?.SelectedTab;

        public void SetFirstTab(object sender, ChangeType changeType) 
            => Display(sender, new UserTabSelectedEventArgs(Section.Tabs[0].Value, changeType));
        public void SetCurrentTab(object sender, ChangeType changeType)
            => Display(sender, new UserTabSelectedEventArgs(Section.GetCurrentTab(), changeType));
        public void SetLastTab(object sender, ChangeType changeType)
            => Display(sender, new UserTabSelectedEventArgs(Section.Tabs[Section.Tabs.Count - 1].Value, changeType));
    }
}

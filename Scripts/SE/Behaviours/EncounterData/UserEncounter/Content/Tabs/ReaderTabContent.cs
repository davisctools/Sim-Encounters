using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    [RequireComponent(typeof(RectTransform))]
    public class ReaderTabContent : UserTabSelectorBehaviour
    {
        public RectTransform RectTransform => (RectTransform)transform;
        public UserTab Tab => UserTabValue?.SelectedTab;
    }
}

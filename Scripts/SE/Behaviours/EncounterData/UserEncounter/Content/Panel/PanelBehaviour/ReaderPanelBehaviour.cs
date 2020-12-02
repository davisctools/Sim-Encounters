using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class ReaderPanelBehaviour : BaseReaderPanelBehaviour
    {
        protected override BaseChildUserPanelsDrawer ChildPanelsDrawer { get => childPanelsDrawer; }
        [SerializeField] private BaseChildUserPanelsDrawer childPanelsDrawer = null;
        protected override BaseUserPinGroupDrawer PinsDrawer { get => pinsDrawer; }
        [SerializeField] private BaseUserPinGroupDrawer pinsDrawer = null;
        protected override bool SetReadOnSelect => setReadOnSelect;
        [SerializeField] private bool setReadOnSelect = true;
    }
}
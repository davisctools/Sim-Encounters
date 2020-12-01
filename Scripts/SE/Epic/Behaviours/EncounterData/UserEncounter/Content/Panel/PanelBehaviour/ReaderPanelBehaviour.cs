using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class ReaderPanelBehaviour : BaseReaderPanelBehaviour
    {
        protected override BaseChildUserPanelsDrawer ChildPanelsDrawer { get => childPanelsDrawer; }
        [SerializeField] private BaseChildUserPanelsDrawer childPanelsDrawer = null;
        protected override BaseUserPinGroupDrawer PinsDrawer { get => pinsDrawer; }
        [SerializeField] private BaseUserPinGroupDrawer pinsDrawer = null;
        public virtual bool SetReadOnSelect { get => setReadOnSelect; set => setReadOnSelect = value; }
        [SerializeField] private bool setReadOnSelect = true;
        public override void Select(object sender, UserPanelSelectedEventArgs eventArgs)
        {
            base.Select(sender, eventArgs);
            if (SetReadOnSelect && eventArgs.Active && !CurrentPanel.IsRead() && !CurrentPanel.HasChildren())
                CurrentPanel.SetRead(true);
        }
    }

}
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class ReaderTabDrawer : UserTabSelectorBehaviour
    {
        public BaseChildUserPanelsDrawer PanelCreator { get => panelCreator; set => panelCreator = value; }
        [SerializeField] private BaseChildUserPanelsDrawer panelCreator;

        public override void Select(object sender, UserTabSelectedEventArgs eventArgs)
        {
            base.Select(sender, eventArgs);
            if (PanelCreator != null && eventArgs.SelectedTab.Panels?.Count > 0)
                PanelCreator.Display(eventArgs.SelectedTab.Panels, eventArgs.ChangeType != ChangeType.Inactive); 
        }
    }
}
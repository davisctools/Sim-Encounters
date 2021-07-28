using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class ReaderTabDrawer : UserTabSelectorBehaviour
    {
        public BaseChildUserPanelsDrawer PanelCreator { get => panelCreator; set => panelCreator = value; }
        [SerializeField] private BaseChildUserPanelsDrawer panelCreator;

        public override void Display(object sender, UserTabSelectedEventArgs eventArgs)
        {
            base.Display(sender, eventArgs);

            var tab = eventArgs.SelectedTab;
            if (PanelCreator != null)
                PanelCreator.Display(tab.Panels, eventArgs.ChangeType != ChangeType.Inactive);

            if (eventArgs.ChangeType != ChangeType.Inactive && tab.Panels.Count == 0)
                tab.SetRead(true);
        }
    }
}
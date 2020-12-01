using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class WriterTab : BaseTabDrawer
    {
        public BaseWriterPanelsDrawer PanelCreator { get => panelCreator; set => panelCreator = value; }
        [SerializeField] private BaseWriterPanelsDrawer panelCreator;

        protected Tab CurrentTab { get; set; }
        public override void Select(object sender, TabSelectedEventArgs eventArgs)
        {
            base.Select(sender, eventArgs);

            CurrentTab = eventArgs.SelectedTab;
            if (CurrentTab.Panels.Count == 0)
                PanelCreator.DrawDefaultChildPanels();
            else
                PanelCreator.DrawChildPanels(CurrentTab.Panels);
        }

        public override Tab Serialize()
        {
            CurrentTab.Panels = PanelCreator.SerializeChildren();
            return CurrentTab;
        }
    }
}
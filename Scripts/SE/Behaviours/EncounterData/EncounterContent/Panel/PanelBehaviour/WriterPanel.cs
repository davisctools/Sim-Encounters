using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class WriterPanel : BaseWriterPanel
    {
        protected override BaseWriterPanelsDrawer ChildPanelCreator { get => childPanelCreator; }
        [SerializeField] private BaseWriterPanelsDrawer childPanelCreator;

        protected override BaseWriterPinsDrawer PinsDrawer { get => pinsDrawer; }
        [SerializeField] private BaseWriterPinsDrawer pinsDrawer;
    }
}
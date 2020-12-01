﻿using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseReaderPanelBehaviour : UserPanelSelectorBehaviour
    {
        protected abstract BaseChildUserPanelsDrawer ChildPanelsDrawer { get; }
        protected abstract BaseUserPinGroupDrawer PinsDrawer { get; }
        public virtual string Type { get => type; set => type = value; }
        [SerializeField] private string type;

        protected UserPanel CurrentPanel { get; set; }

        public override void Select(object sender, UserPanelSelectedEventArgs eventArgs)
        {
            base.Select(sender, eventArgs);
            CurrentPanel = eventArgs.SelectedPanel;

            if (ChildPanelsDrawer != null && CurrentPanel.ChildPanels?.Count > 0)
                ChildPanelsDrawer.Display(eventArgs.SelectedPanel.ChildPanels, eventArgs.Active);
            if (PinsDrawer != null)
                PinsDrawer.Display(eventArgs.SelectedPanel.PinGroup);

            if (eventArgs.Active)
                eventArgs.SelectedPanel.SetRead(true);
        }

        public class Factory : PlaceholderFactory<UnityEngine.Object, BaseReaderPanelBehaviour> { }
    }

}
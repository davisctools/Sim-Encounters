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
        protected abstract bool SetReadOnSelect { get; }

        public override void Display(object sender, UserPanelSelectedEventArgs eventArgs)
        {
            base.Display(sender, eventArgs);
            CurrentPanel = eventArgs.SelectedPanel;

            if (ChildPanelsDrawer != null)
                ChildPanelsDrawer.Display(eventArgs.SelectedPanel.ChildPanels, eventArgs.Active);
            if (PinsDrawer != null)
                PinsDrawer.Display(eventArgs.SelectedPanel.PinGroup);

            if (SetReadOnSelect && eventArgs.Active && !CurrentPanel.IsRead() && !CurrentPanel.HasChildren())
                CurrentPanel.SetRead(true);
        }

        public class Factory : PlaceholderFactory<Object, BaseReaderPanelBehaviour> { }
    }

}
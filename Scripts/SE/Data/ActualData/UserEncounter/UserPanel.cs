using ClinicalTools.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClinicalTools.SimEncounters
{
    public class UserPanel
    {
        public UserEncounter Encounter { get; }
        public Panel Data { get; }
        protected PanelStatus Status { get; }
        public UserPinGroup PinGroup { get; }

        public event Action StatusChanged;

        public UserPanel(UserEncounter encounter, Panel data, PanelStatus status)
        {
            Encounter = encounter;
            Data = data;
            Status = status;
            if (data.Pins != null && data.Pins.HasPin()) {
                PinGroup = new UserPinGroup(encounter, data.Pins, status.PinGroupStatus);
                PinGroup.StatusChanged += UpdateIsRead;
            }

            foreach (var panel in data.ChildPanels) {
                var userPanel = new UserPanel(encounter, panel.Value, status.GetChildPanelStatus(panel.Key));
                userPanel.StatusChanged += UpdateIsRead;
                ChildPanels.Add(panel.Key, userPanel);
            }
        }

        protected virtual void UpdateIsRead()
        {
            if (!Status.Read && !ChildPanels.Values.Any(p => !p.IsRead()))
                SetRead(true);
        }

        public bool IsRead() => Status.Read;
        public void SetRead(bool read)
        {
            if (Status.Read == read)
                return;

            Status.Read = read;
            StatusChanged?.Invoke();
        }
        public void SetChildPanelsRead(bool read)
        {
            if (ChildPanels.Count == 0) {
                SetRead(true);
                return;
            }

            foreach (var childPanel in ChildPanels.Values)
                childPanel.SetChildPanelsRead(read);
        }

        public virtual OrderedCollection<UserPanel> ChildPanels { get; } = new OrderedCollection<UserPanel>();
        public virtual bool HasChildren() => ChildPanels.Count > 0;
        public virtual IEnumerable<UserPanel> GetChildPanels() => ChildPanels.Values;
        public virtual UserPanel GetChildPanel(string key) => ChildPanels[key];
    }
}
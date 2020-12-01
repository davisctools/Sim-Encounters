using ClinicalTools.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClinicalTools.SimEncounters
{
    public class UserTab
    {
        public UserEncounter Encounter { get; }
        public Tab Data { get; }
        protected TabStatus Status { get; }
        public event Action StatusChanged;
        public UserTab(UserEncounter encounter, Tab data, TabStatus status)
        {
            Encounter = encounter;
            Data = data;
            Status = status;

            foreach (var panel in data.Panels) {
                var userPanel = new UserPanel(encounter, panel.Value, status.GetPanelStatus(panel.Key));
                userPanel.StatusChanged += UpdateIsRead;
                Panels.Add(panel.Key, userPanel);
            }
        }

        protected virtual void UpdateIsRead()
        {
            if (!Status.Read && !Panels.Values.Any(p => !p.IsRead()))
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

        public virtual OrderedCollection<UserPanel> Panels { get; } = new OrderedCollection<UserPanel>();
        public virtual IEnumerable<UserPanel> GetPanels() => Panels.Values;
        public virtual UserPanel GetPanel(string key) => Panels[key];
    }
}
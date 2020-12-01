using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class TabStatus
    {
        public bool Read { get; set; }
        protected Dictionary<string, PanelStatus> Panels { get; } = new Dictionary<string, PanelStatus>();
        public virtual void AddPanelStatus(string key, PanelStatus status)
        {
            if (!Panels.ContainsKey(key))
                Panels.Add(key, status);
            else
                Debug.LogError($"Tab already has panel status with key ({key})");
        }
        public virtual PanelStatus GetPanelStatus(string key)
        {
            if (Panels.ContainsKey(key))
                return Panels[key];

            var panelStatus = new PanelStatus {
                Read = Read
            };
            Panels.Add(key, panelStatus);
            return panelStatus;
        }
        public virtual IEnumerable<KeyValuePair<string, PanelStatus>> PanelStatuses => Panels;
    }
}
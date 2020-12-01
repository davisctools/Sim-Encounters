using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public class PanelStatus
    {
        public bool Read { get; set; }

        private PinGroupStatus pinDataStatus;
        public PinGroupStatus PinGroupStatus
        {
            get {
                if (pinDataStatus == null)
                    pinDataStatus = new PinGroupStatus { Read = Read };
                return pinDataStatus;
            }
            set => pinDataStatus = value;
        }

        protected Dictionary<string, PanelStatus> Panels { get; } = new Dictionary<string, PanelStatus>();
        public virtual void AddPanelStatus(string key, PanelStatus status)
        {
            if (Panels.ContainsKey(key))
                UnityEngine.Debug.LogError($"panel already has key named {key}");
            else
                Panels.Add(key, status);
        }

        public virtual PanelStatus GetChildPanelStatus(string key)
        {
            if (Panels.ContainsKey(key))
                return Panels[key];

            var panelStatus = new PanelStatus {
                Read = Read
            };
            Panels.Add(key, panelStatus);
            return panelStatus;
        }
        public virtual IEnumerable<KeyValuePair<string, PanelStatus>> ChildPanelStatuses => Panels;
    }
}
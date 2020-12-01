using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public class QuizStatus
    {
        public bool Read { get; set; }
        protected Dictionary<string, PanelStatus> Panels { get; } = new Dictionary<string, PanelStatus>();
        public virtual void AddPanelStatus(string key, PanelStatus status) => Panels.Add(key, status);

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
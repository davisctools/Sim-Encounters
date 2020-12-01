using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public class SectionStatus
    {
        public virtual bool Read { get; set; }
        protected Dictionary<string, TabStatus> Tabs { get; } = new Dictionary<string, TabStatus>();
        public virtual void AddTabStatus(string key, TabStatus status) => Tabs.Add(key, status);
        public virtual TabStatus GetTabStatus(string key)
        {
            if (Tabs.ContainsKey(key))
                return Tabs[key];

            var tabStatus = new TabStatus {
                Read = Read
            };
            Tabs.Add(key, tabStatus);
            return tabStatus;
        }

        public virtual IEnumerable<KeyValuePair<string, TabStatus>> TabStatuses => Tabs;
    }
}
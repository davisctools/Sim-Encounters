using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class EncounterContentStatus
    {
        public virtual bool Read { get; set; }

        protected Dictionary<string, SectionStatus> Sections { get; } = new Dictionary<string, SectionStatus>();
        public virtual void AddSectionStatus(string key, SectionStatus status)
        {
            if (Sections.ContainsKey(key))
                Debug.LogWarning($"Duplicate section status key ({key})");
            else
                Sections.Add(key, status);
        }

        public virtual SectionStatus GetSectionStatus(string key)
        {
            if (Sections.ContainsKey(key))
                return Sections[key];

            var sectionStatus = new SectionStatus {
                Read = Read
            };
            Sections.Add(key, sectionStatus);
            return sectionStatus;
        }

        public virtual IEnumerable<KeyValuePair<string, SectionStatus>> SectionStatuses => Sections;
    }
}
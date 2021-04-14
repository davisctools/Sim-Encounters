using ClinicalTools.Collections;
using System;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class EncounterContentData
    {
        public OrderedCollection<Character> Characters { get; } = new OrderedCollection<Character>();
        public KeyedCollection<EncounterImage> Images { get; } = new KeyedCollection<EncounterImage>();
        public virtual KeyedCollection<Sprite> Icons { get; } = new KeyedCollection<Sprite>();

        public virtual int CurrentSectionIndex { get; set; }
        public virtual Section GetCurrentSection() => Sections[CurrentSectionIndex].Value;
        public virtual string GetCurrentSectionKey() => Sections[CurrentSectionIndex].Key;
        public virtual void SetCurrentSection(Section section)
        {
            if (!Sections.Contains(section))
                throw new Exception($"Passed section is not contained in the collection of sections.");
            CurrentSectionIndex = Sections.IndexOf(section);
        }

        public virtual OrderedCollection<Section> Sections { get; } = new OrderedCollection<Section>();

        public int GetCurrentTabNumber()
        {
            var tabNumber = 1;
            for (int i = 0; i < CurrentSectionIndex; i++)
                tabNumber += Sections[i].Value.Tabs.Count;
            tabNumber += Sections[CurrentSectionIndex].Value.CurrentTabIndex;
            return tabNumber;
        }
        public int GetTabCount()
        {
            var tabNumber = 0;
            foreach (var section in Sections)
                tabNumber += section.Value.Tabs.Count;

            return tabNumber;
        }
    }
}
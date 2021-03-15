using ClinicalTools.Collections;
using System;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{

    public class Section
    {
        public int CurrentTabIndex { 
            get; 
            set; 
        }
        public virtual Tab GetCurrentTab()
        {
            if (Tabs.Count == 0)
                return null;
            return Tabs[CurrentTabIndex].Value;
        }
        public virtual string GetCurrentTabKey()
        {
            if (Tabs.Count == 0)
                return null;
            return Tabs[CurrentTabIndex].Key;
        }
        public virtual void SetCurrentTab(Tab tab)
        {
            if (!Tabs.Contains(tab))
                throw new Exception($"Passed tab is not contained in the collection of tabs.");
            CurrentTabIndex = Tabs.IndexOf(tab);
        }

        public virtual string Name { get; set; }
        public virtual string IconKey { get; set; }
        public virtual Color Color { get; set; }

        public virtual OrderedCollection<Tab> Tabs { get; } = new OrderedCollection<Tab>();

        public Section(string name, string iconKey, Color color)
        {
            Name = name;
            IconKey = iconKey;
            Color = color;
        }

        public int MoveToNextTab()
        {
            if (CurrentTabIndex < Tabs.Count - 1)
                CurrentTabIndex++;
            return CurrentTabIndex;
        }
        public int MoveToPreviousTab()
        {
            if (CurrentTabIndex > 0)
                CurrentTabIndex--;
            return CurrentTabIndex;
        }
    }
}
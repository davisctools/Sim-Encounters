using System;

namespace ClinicalTools.SimEncounters
{
    public class EncounterFilterItem
    {
        public virtual event Action<EncounterFilterItem> Deleted;

        public virtual string Display { get; set; }

        public EncounterFilterItem() { }
        public EncounterFilterItem(string display) => Display = display;

        public virtual void Delete() => Deleted?.Invoke(this);
    }
}
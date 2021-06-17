using UnityEngine;
using System;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public class FilterGroupUI : EncounterFilterBehaviour
    {
        public override Filter<MenuEncounter> EncounterFilter => FilterGroups;

        public override event Action<Filter<MenuEncounter>> FilterChanged;

        [SerializeField] private List<EncounterFilterBehaviour> encounterFilters;
        public List<EncounterFilterBehaviour> EncounterFilters { get => encounterFilters; set => encounterFilters = value; }

        protected bool ChangeCallerDisabled { get; set; } = false;

        protected void Awake()
        {
            foreach (var encounterFilter in EncounterFilters)
                encounterFilter.FilterChanged += OnFilterChanged;
        }

        protected bool FilterGroups(MenuEncounter encounter)
        {
            foreach (var encounterFilter in EncounterFilters) {
                if (!encounterFilter.EncounterFilter(encounter))
                    return false;
            }

            return true;
        }

        protected bool ChildChanged { get; set; } = false;
        protected virtual void OnFilterChanged(Filter<MenuEncounter> filter)
        {
            if (!ChangeCallerDisabled)
                FilterChanged?.Invoke(EncounterFilter);
            else
                ChildChanged = true;
        }

        public override void Clear()
        {
            foreach (var encounterFilter in EncounterFilters)
                encounterFilter.Clear();
            if (ChildChanged) {
                FilterChanged?.Invoke(EncounterFilter);
                ChildChanged = false;
            }
        }
    }
}
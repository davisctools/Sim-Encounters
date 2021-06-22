using UnityEngine;
using System;
using System.Collections.Generic;
using ClinicalTools.UI;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    public class AudienceFilterUI : EncounterFilterBehaviour
    {
        public List<LabeledToggle> AudienceToggles { get => audienceToggles; set => audienceToggles = value; }
        [SerializeField] private List<LabeledToggle> audienceToggles;

        public override Filter<MenuEncounter> EncounterFilter => FilterAudience;
        public override event Action<Filter<MenuEncounter>> FilterChanged;

        protected bool ChangeCallerDisabled { get; set; } = false;
        protected Dictionary<string, AudienceFilterItem> FilterItems { get; } = new Dictionary<string, AudienceFilterItem>();


        protected virtual void Awake()
        {
            foreach (var toggle in AudienceToggles) {
                var audienceToggle = toggle.Toggle;
                toggle.Toggle.onValueChanged.AddListener((isOn) => ToggleAudience(audienceToggle, toggle.Label.text));
            }
        }

        protected virtual void ToggleAudience(Toggle toggle, string audience)
        {
            if (!toggle.isOn) {
                FilterDeleted(audience);
                return;
            }

            if (FilterItems.ContainsKey(audience))
                return;

            var display = audience.Trim().ToUpperInvariant();
            AudienceFilterItem filterItem = new AudienceFilterItem(toggle, audience, display);
            filterItem.Deleted += FilterDeleted;
            FilterItems.Add(audience, filterItem);

            if (!ChangeCallerDisabled)
                FilterChanged?.Invoke(EncounterFilter);
        }

        protected virtual void FilterDeleted(EncounterFilterItem filterItem) => FilterDeleted(filterItem.Display);
        protected virtual void FilterDeleted(string filterKey)
        {
            if (!FilterItems.ContainsKey(filterKey))
                return;
            var filterItem = FilterItems[filterKey];
            FilterItems.Remove(filterKey);
            filterItem.Toggle.isOn = false;

            if (!ChangeCallerDisabled)
                FilterChanged?.Invoke(EncounterFilter);
        }

        protected virtual bool FilterAudience(MenuEncounter encounter)
        {
            if (FilterItems.Count == 0)
                return true;

            var audience = encounter.GetLatestMetadata().Audience.ToUpper();
            foreach (var filteredAudience in FilterItems.Values) {
                if (audience.Contains(filteredAudience.AudienceCompareText))
                    return true;
            }

            return false;
        }

        public override IEnumerable<EncounterFilterItem> GetFilterItems() => FilterItems.Values;

        public override void Clear()
        {
            if (FilterItems.Count == 0)
                return;

            ChangeCallerDisabled = true;
            foreach (var toggle in AudienceToggles) {
                if (!toggle.Toggle.isOn)
                    continue;

                toggle.Toggle.isOn = false;
            }
            ChangeCallerDisabled = false;

            FilterChanged?.Invoke(EncounterFilter);
        }
    }
}
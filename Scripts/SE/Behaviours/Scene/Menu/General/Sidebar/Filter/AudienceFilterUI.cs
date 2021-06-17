using UnityEngine;
using System;
using System.Collections.Generic;
using ClinicalTools.UI;

namespace ClinicalTools.SimEncounters
{
    public class AudienceFilterUI : EncounterFilterBehaviour
    {
        public List<LabeledToggle> AudienceToggles { get => audienceToggles; set => audienceToggles = value; }
        [SerializeField] private List<LabeledToggle> audienceToggles;

        public override Filter<MenuEncounter> EncounterFilter => FilterAudience;
        public override event Action<Filter<MenuEncounter>> FilterChanged;

        protected bool ChangeCallerDisabled { get; set; } = false;
        protected List<string> FilteredAudiences { get; } = new List<string>();

        protected void Awake()
        {
            foreach (var toggle in AudienceToggles)
                toggle.Toggle.onValueChanged.AddListener((isOn) => ToggleDifficulty(isOn, toggle.Label.text));
        }

        protected void ToggleDifficulty(bool isOn, string difficulty)
        {
            if (isOn)
                FilteredAudiences.Add(difficulty);
            else
                FilteredAudiences.Remove(difficulty);

            if (!ChangeCallerDisabled)
                FilterChanged?.Invoke(EncounterFilter);
        }

        protected bool FilterAudience(MenuEncounter encounter)
        {
            if (FilteredAudiences.Count == 0)
                return true;

            var audience = encounter.GetLatestMetadata().Audience.ToUpper();
            foreach (var filteredAudience in FilteredAudiences) {
                if (audience.Contains(filteredAudience.ToUpper()))
                    return true;
            }

            return false;
        }

        public override void Clear()
        {
            ChangeCallerDisabled = true;

            var filterChanged = false;
            foreach (var toggle in AudienceToggles) {
                if (!toggle.Toggle.isOn)
                    continue;

                toggle.Toggle.isOn = false;
                filterChanged = true;
            }
            ChangeCallerDisabled = false;

            if (filterChanged)
                FilterChanged?.Invoke(EncounterFilter);
        }
    }
}
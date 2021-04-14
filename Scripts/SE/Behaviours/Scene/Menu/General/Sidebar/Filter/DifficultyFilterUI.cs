using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;


namespace ClinicalTools.SimEncounters
{
    public class DifficultyFilterUI : EncounterFilterBehaviour
    {
        public Toggle Beginner { get => beginner; set => beginner = value; }
        [SerializeField] private Toggle beginner;
        public Toggle Intermediate { get => intermediate; set => intermediate = value; }
        [SerializeField] private Toggle intermediate;
        public Toggle Advanced { get => advanced; set => advanced = value; }
        [SerializeField] private Toggle advanced;

        public override Filter<MenuEncounter> EncounterFilter => FilterDifficulty;
        public override event Action<Filter<MenuEncounter>> FilterChanged;

        protected List<EncounterDifficulty> FilteredDifficulties { get; } = new List<EncounterDifficulty>();

        public void Awake()
        {
            Beginner.onValueChanged.AddListener((isOn) => ToggleDifficulty(isOn, EncounterDifficulty.Beginner));
            Intermediate.onValueChanged.AddListener((isOn) => ToggleDifficulty(isOn, EncounterDifficulty.Intermediate));
            Advanced.onValueChanged.AddListener((isOn) => ToggleDifficulty(isOn, EncounterDifficulty.Advanced));
        }

        protected void ToggleDifficulty(bool isOn, EncounterDifficulty difficulty)
        {
            if (isOn)
                FilteredDifficulties.Add(difficulty);
            else
                FilteredDifficulties.Remove(difficulty);

            FilterChanged?.Invoke(EncounterFilter);
        }

        protected bool FilterDifficulty(MenuEncounter encounter)
        {
            if (FilteredDifficulties.Count == 0)
                return true;

            return FilteredDifficulties.Contains(encounter.GetLatestMetadata().Difficulty);
        }

        public override void Clear()
        {
            Beginner.isOn = false;
            Intermediate.isOn = false;
            Advanced.isOn = false;
        }
    }
}
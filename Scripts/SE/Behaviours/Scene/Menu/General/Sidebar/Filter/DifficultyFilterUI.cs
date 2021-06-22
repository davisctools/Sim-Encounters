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

        protected bool ChangeCallerDisabled { get; set; } = false;
        protected Dictionary<Difficulty, DifficultyFilterItem> FilterItems { get; } = new Dictionary<Difficulty, DifficultyFilterItem>();

        public void Awake()
        {
            Beginner.onValueChanged.AddListener((isOn) => ToggleDifficulty(Beginner, Difficulty.Beginner));
            Intermediate.onValueChanged.AddListener((isOn) => ToggleDifficulty(Intermediate, Difficulty.Intermediate));
            Advanced.onValueChanged.AddListener((isOn) => ToggleDifficulty(Advanced, Difficulty.Advanced));
        }

        protected void ToggleDifficulty(Toggle toggle, Difficulty difficulty)
        {
            if (!toggle.isOn) {
                FilterDeleted(difficulty);
                return;
            }

            if (FilterItems.ContainsKey(difficulty))
                return;

            var display = GetDifficultyDisplay(difficulty);
            DifficultyFilterItem filterItem = new DifficultyFilterItem(toggle, display, difficulty);
            filterItem.Deleted += FilterDeleted;
            FilterItems.Add(difficulty, filterItem);

            if (!ChangeCallerDisabled)
                FilterChanged?.Invoke(EncounterFilter);
        }

        protected virtual void FilterDeleted(EncounterFilterItem filterItem)
        {
            if (filterItem is DifficultyFilterItem difficultyFilterItem)
                FilterDeleted(difficultyFilterItem.Difficulty);
        }

        protected virtual void FilterDeleted(Difficulty filterKey)
        {
            if (!FilterItems.ContainsKey(filterKey))
                return;
            var filterItem = FilterItems[filterKey];
            FilterItems.Remove(filterKey);
            filterItem.Toggle.isOn = false;

            if (!ChangeCallerDisabled)
                FilterChanged?.Invoke(EncounterFilter);
        }

        protected virtual string GetDifficultyDisplay(Difficulty difficulty)
        {
            switch (difficulty) {
                case Difficulty.Beginner:
                    return "Beginner";
                case Difficulty.Intermediate:
                    return "Intermediate";
                case Difficulty.Advanced:
                    return "Advanced";
                default:
                    return "";
            }
        }

        public override IEnumerable<EncounterFilterItem> GetFilterItems() => FilterItems.Values;

        protected bool FilterDifficulty(MenuEncounter encounter)
            => (FilterItems.Count == 0) ? true : FilterItems.ContainsKey(encounter.GetLatestMetadata().Difficulty);

        public override void Clear()
        {
            if (FilterItems.Count == 0)
                return;

            ChangeCallerDisabled = true;

            Beginner.isOn = false;
            Intermediate.isOn = false;
            Advanced.isOn = false;

            ChangeCallerDisabled = false;

            FilterChanged?.Invoke(EncounterFilter);
        }
    }
}
using UnityEngine;
using System;
using TMPro;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public class SearchFilterUI : EncounterFilterBehaviour
    {
        public override Filter<MenuEncounter> EncounterFilter => FilterSearchTerm;
        public override event Action<Filter<MenuEncounter>> FilterChanged;


        [SerializeField] private TMP_InputField searchField;
        public virtual TMP_InputField SearchField { get => searchField; set => searchField = value; }

        protected string SearchTerm { get; set; }

        protected void Awake()
        {
            SearchField.onValueChanged.AddListener(SearchTermChanged);
        }

        protected void SearchTermChanged(string searchTerm)
        {
            SearchTerm = searchTerm;
            FilterChanged?.Invoke(EncounterFilter);
        }

        protected bool FilterSearchTerm(MenuEncounter encounter)
        {
            if (string.IsNullOrWhiteSpace(SearchTerm))
                return true;

            return encounter.GetLatestMetadata().Title.ToLower().Contains(SearchTerm.ToLower().Trim());
        }

        public override void Clear()
        {
            SearchField.text = "";
        }

        // TODO: create an actual filter item based on the text search filter
        public override IEnumerable<EncounterFilterItem> GetFilterItems() => new List<EncounterFilterItem>();
    }
}
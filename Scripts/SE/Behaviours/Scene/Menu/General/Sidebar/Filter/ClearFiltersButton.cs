using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    [RequireComponent(typeof(Button))]
    public class ClearFiltersButton : MonoBehaviour
    {
        protected EncounterFilterBehaviour FilterBehaviour { get; set; }
        protected virtual void Inject(EncounterFilterBehaviour filterBehaviour) => FilterBehaviour = filterBehaviour;

        protected virtual void Awake()
        {
            var button = GetComponent<Button>();
            button.onClick.AddListener(ClearFilters);
        }

        protected virtual void ClearFilters() => FilterBehaviour.Clear();

    }
}
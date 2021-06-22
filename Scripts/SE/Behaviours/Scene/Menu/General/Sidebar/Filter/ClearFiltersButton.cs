using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    [RequireComponent(typeof(Button))]
    public class ClearFiltersButton : MonoBehaviour
    {
        protected EncounterFilterBehaviour FilterBehaviour { get; set; }
        [Inject] public virtual void Inject(EncounterFilterBehaviour filterBehaviour) => FilterBehaviour = filterBehaviour;

        protected virtual void Awake()
        {
            var button = GetComponent<Button>();
            button.onClick.AddListener(ClearFilters);
        }

        protected virtual void ClearFilters() => FilterBehaviour.Clear();

    }
}
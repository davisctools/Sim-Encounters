using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class EncounterSearchUI : MonoBehaviour
    {
        public virtual FilterGroupUI Filters { get => filters; set => filters = value; }
        [SerializeField] private FilterGroupUI filters;
        public virtual SortingOrderUI SortingOrder { get => sortingOrder; set => sortingOrder = value; }
        [SerializeField] private SortingOrderUI sortingOrder;
    }
}
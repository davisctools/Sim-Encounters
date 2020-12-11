using TMPro;
using UnityEngine;

namespace ClinicalTools.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class LabelNumberedBySiblingIndex : MonoBehaviour
    {
        [SerializeField] private Transform transformToGetSiblingIndexOf;

        private void Start()
            => GetComponent<TextMeshProUGUI>().text = $"{transformToGetSiblingIndexOf.GetSiblingIndex() + 1}.";
    }
}
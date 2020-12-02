using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class TabTypeButtonUI : MonoBehaviour
    {
        [field: SerializeField] public TextMeshProUGUI Label { get; set; }
        [field: SerializeField] public Toggle Toggle { get; set; }

        internal void Deselect() => Toggle.isOn = false;

        public class Factory : PlaceholderFactory<TabTypeButtonUI> { }
    }
}
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ClinicalTools.UI
{
    public class LabeledToggle : MonoBehaviour
    {
        public Toggle Toggle { get => toggle; set => toggle = value; }
        [SerializeField] private Toggle toggle;

        public TextMeshProUGUI Label { get => label; set => label = value; }
        [SerializeField] private TextMeshProUGUI label;
    }
}
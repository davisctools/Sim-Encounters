using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.UI
{
    [RequireComponent(typeof(Toggle))]
    public class OnColorToggle : MonoBehaviour
    {
        [field: SerializeField] public Color OnColor { get; set; }

        private void Start()
        {
            var toggle = GetComponent<Toggle>();
            var offColor = toggle.colors.normalColor;

            toggle.onValueChanged.AddListener((selected) => Selected(toggle, offColor));
        }

        private void Selected(Toggle toggle, Color offColor)
        {
            var colorBlock = toggle.colors;
            colorBlock.normalColor = (toggle.isOn) ? OnColor : offColor;
            toggle.colors = colorBlock;
        }
    }
}
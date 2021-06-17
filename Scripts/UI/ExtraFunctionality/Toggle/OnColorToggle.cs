using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.UI
{
    [ExecuteAlways]
    [RequireComponent(typeof(Toggle))]
    public class OnColorToggle : ColorBehaviour
    {
        protected Color OnColor { get; set; }
        protected Color OffColor { get; set; }
        protected Color OffSelectedColor { get; set; }
        protected Toggle Toggle { get; set; }

        protected override void Start()
        {
            Toggle = GetComponent<Toggle>();
            OffColor = Toggle.image.color;

            Toggle.onValueChanged.AddListener(Selected);

            base.Start();
        }

        protected override void SetColor(Color color)
        {
            OnColor = color;
            Selected(Toggle.isOn);
        }

        private void Selected(bool isOn)
        {
            Toggle.image.color = isOn ? OnColor : OffColor;
        }
    }
}
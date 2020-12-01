using TMPro;
using UnityEngine;

namespace ClinicalTools.UI
{
    [ExecuteAlways]
    public class ColorLabel : ColorBehaviour
    {
        protected override void SetColor(Color color) => GetComponent<TextMeshProUGUI>().color = color;
    }
}
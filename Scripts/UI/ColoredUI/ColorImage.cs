using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.UI
{
    [ExecuteAlways]
    [RequireComponent(typeof(Image))]
    public class ColorImage : ColorBehaviour
    {
        protected override void SetColor(Color color) => GetComponent<Image>().color = color;
    }
}
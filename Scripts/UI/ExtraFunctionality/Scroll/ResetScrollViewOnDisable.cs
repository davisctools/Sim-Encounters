using ClinicalTools.UI;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.UI
{
    public class ResetScrollViewOnDisable : MonoBehaviour
    {
        public ScrollRect ScrollRect { get => scrollRect; set => scrollRect = value; }
        [SerializeField] private ScrollRect scrollRect;
        public ScrollRectGradient ScrollGradient { get => scrollGradient; set => scrollGradient = value; }
        [SerializeField] private ScrollRectGradient scrollGradient;

        protected virtual void OnDisable()
        {
            if (ScrollRect != null)
                ScrollRect.verticalNormalizedPosition = 1;
            if (ScrollGradient != null)
                ScrollGradient.ResetGradients();
        }
    }
}
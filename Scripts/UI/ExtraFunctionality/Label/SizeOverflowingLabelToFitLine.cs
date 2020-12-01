using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ClinicalTools.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class SizeOverflowingLabelToFitLine : UIBehaviour
    {
        private bool awakeCalled = false;
        private TextMeshProUGUI label;
        private LayoutElement layoutElement;

        protected override void Awake()
        {
            label = GetComponent<TextMeshProUGUI>();
            layoutElement = GetComponent<LayoutElement>();
            awakeCalled = true;
            base.Awake();
        }

        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();

            if (!awakeCalled)
                return;

        }

        Rect rect;
        protected void Update()
        {
            if (label.linkedTextComponent == null || label.firstOverflowCharacterIndex <= 0)
                return;

            Rect currentRect = ((RectTransform)transform).rect;
            if (rect.height == currentRect.height && rect.width == currentRect.width)
                return;
            rect = currentRect;

            layoutElement.minHeight = GetHeight();
        }

        protected float GetHeight()
        {
            string str = "";
            while (true) {
                str += "a\n";
                var y = label.GetPreferredValues(str);
                if (y.y >= rect.height)
                    return y.y;
            }
        }
    }
}
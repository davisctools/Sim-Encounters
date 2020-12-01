using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ClinicalTools.UI
{
    [ExecuteAlways]
    public class HeightAsProportionToWidth : UIBehaviour
    {
        public float HeightPerWidth { get => heightPerWidth; set => heightPerWidth = value; }
        [SerializeField] private float heightPerWidth = 1;
        public float InitialWidthProportionToParent { get => initialWidthProportionToParent; set => initialWidthProportionToParent = value; }
        [SerializeField] private float initialWidthProportionToParent = 0;

        // I would assign LayoutElement in Awake, but Awake isn't always guaranteed in Editor mode
        private bool checkedForLayoutElement;
        private LayoutElement layoutElement;
        protected virtual LayoutElement LayoutElement {
            get {
                if (!checkedForLayoutElement) {
                    checkedForLayoutElement = true;
                    layoutElement = GetComponent<LayoutElement>();
                }

                return layoutElement;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            if (InitialWidthProportionToParent > 0 && transform.parent != null)
                width = ((RectTransform)transform.parent).rect.width * InitialWidthProportionToParent;

            UpdateHeight();
        }

        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();

            var currentWidth = ((RectTransform)transform).rect.width;
            if (Mathf.Abs(currentWidth - width) < Tolerance)
                return;

            width = currentWidth;
            UpdateHeight();
        }

        private float lastHeightPerHeight;
        protected virtual void Update()
        {
            if (lastHeightPerHeight != HeightPerWidth)
                UpdateHeight();
        }

        private float width;
        private const float Tolerance = .0001f;
        private void UpdateHeight()
        {
            if (width < Tolerance)
                width = ((RectTransform)transform).rect.width;

            lastHeightPerHeight = HeightPerWidth;

            var height = HeightPerWidth * width;
            if (LayoutElement == null || LayoutElement.ignoreLayout) {
                ((RectTransform)transform).SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
                return;
            }

            LayoutElement.preferredHeight = height;
        }
    }
}
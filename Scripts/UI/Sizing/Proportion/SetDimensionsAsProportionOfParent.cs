using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ClinicalTools.UI
{
    public class SetDimensionsAsProportionOfParent : UIBehaviour
    {
        public float HeightProportion { get => heightProportion; set => heightProportion = value; }
        [SerializeField] private float heightProportion;
        public float WidthProportion { get => widthProportion; set => widthProportion = value; }
        [SerializeField] private float widthProportion;

        // I would assign LayoutElement in Awake, but Awake isn't always guaranteed in Editor mode
        private bool checkedForLayoutElement;
        private LayoutElement layoutElement;
        protected virtual LayoutElement LayoutElement
        {
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
            if (transform.parent == null)
                return;

            parentRect = ((RectTransform)transform.parent).rect;
            UpdateWidth();
            UpdateHeight();
        }

        private Rect parentRect;
        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();

            if (transform.parent == null)
                return;

            parentRect = ((RectTransform)transform.parent).rect;
            UpdateWidth();
            UpdateHeight();
        }

        private float lastHeightProportion, lastWidthProportion;
        protected virtual void Update()
        {
            if (transform.parent == null)
                return;

            if (parentRect.width < Tolerance && parentRect.height < Tolerance)
                parentRect = ((RectTransform)transform.parent).rect;
            if (lastWidthProportion != WidthProportion)
                UpdateWidth();
            if (lastHeightProportion != HeightProportion)
                UpdateHeight();
        }

        private const float Tolerance = .0001f;
        private void UpdateWidth()
        {
            lastWidthProportion = WidthProportion;
            if (WidthProportion < Tolerance)
                return;

            var width = WidthProportion * parentRect.width;
            if (LayoutElement == null || LayoutElement.ignoreLayout)
                ((RectTransform)transform).SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
            else
                LayoutElement.preferredWidth = width;
        }
        private void UpdateHeight()
        {
            lastHeightProportion = HeightProportion;
            if (HeightProportion < Tolerance)
                return;

            var height = HeightProportion * parentRect.height;
            if (LayoutElement == null || LayoutElement.ignoreLayout)
                ((RectTransform)transform).SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
            else
                LayoutElement.preferredHeight = height;
        }
    }
}
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ClinicalTools.UI
{
    [ExecuteAlways]
    public class WidthAsProportionToHeight : UIBehaviour
    {
        public float WidthPerHeight { get => widthPerHeight; set => widthPerHeight = value; }
        [SerializeField] private float widthPerHeight = 1;
        public float InitialHeightProportionToParent { get => initialHeightProportionToParent; set => initialHeightProportionToParent = value; }
        [SerializeField] private float initialHeightProportionToParent = 0;

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

        protected LayoutGroup ParentGroup { get; set; }

        protected override void Awake()
        {
            base.Awake();

            if (transform.parent != null)
                ParentGroup = transform.parent.GetComponent<LayoutGroup>();

            if (InitialHeightProportionToParent > 0 && transform.parent != null)
                height = ((RectTransform)transform.parent).rect.height * InitialHeightProportionToParent;

            UpdateWidth();
        }

        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();

            var currentHeight = ((RectTransform)transform).rect.height;
            if (Mathf.Abs(currentHeight - height) < Tolerance)
                return;

            height = currentHeight;
            UpdateWidth();
        }

        private float lastWidthPerHeight;
        protected virtual void Update()
        {
            if (lastWidthPerHeight != WidthPerHeight)
                UpdateWidth();
        }

        private float height;
        private const float Tolerance = .0001f;
        private void UpdateWidth()
        {
            if (height < Tolerance)
                height = ((RectTransform)transform).rect.height;

            lastWidthPerHeight = WidthPerHeight;

            var width = WidthPerHeight * height;
            if (LayoutElement == null || LayoutElement.ignoreLayout) {
                ((RectTransform)transform).SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
                return;
            }

            LayoutElement.preferredWidth = width;

            if (ParentGroup != null)
                SetDirty((RectTransform)ParentGroup.transform);
        }


        protected void SetDirty(RectTransform rectTransform)
        {
            if (!IsActive())
                return;

            if (!CanvasUpdateRegistry.IsRebuildingLayout())
                LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
            else
                StartCoroutine(DelayedSetDirty(rectTransform));
        }

        protected IEnumerator DelayedSetDirty(RectTransform rectTransform)
        {
            yield return null;
            LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
        }
    }
}
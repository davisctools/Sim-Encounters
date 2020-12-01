using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ClinicalTools.UI
{
    [ExecuteAlways]
    public class UnscaledDimensions : UIBehaviour
    {
        private const float DefaultScreenHeight = 1030;

        protected RectTransform RectTransform => (RectTransform)transform;

        public float Height { get => height; set => height = value; }
        [SerializeField] private float height;
        public float Width { get => width; set => width = value; }
        [SerializeField] private float width;

        private bool layoutElementInitialized;
        private LayoutElement layoutElement;
        public LayoutElement LayoutElement {
            get {
                if (!layoutElementInitialized) {
                    layoutElement = GetComponent<LayoutElement>();
                    layoutElementInitialized = true;
                }
                return layoutElement;
            }
        }

        protected Vector3 LossyScale { get; set; }
        protected float LastHeight { get; set; }
        protected float LastWidth { get; set; }

        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();
            Update();
        }

        protected override void Awake()
        {
            base.Awake();
            UpdateSize();
        }

        protected override void Start()
        {
            base.Start();
            Update();
        }

        protected virtual void Update()
        {
            if (LossyScale == transform.lossyScale && LastHeight == Height && LastWidth == Width)
                return;

            UpdateSize();
        }

        private float heightProportion;
        private const float Tolerance = .001f;
        protected virtual void UpdateSize()
        {
            LossyScale = transform.lossyScale;
            LastHeight = Height;
            LastWidth = Width;
            var scaledScreenHeight = Screen.height / LossyScale.y;
            heightProportion = scaledScreenHeight / DefaultScreenHeight;

            if (LayoutElement != null && !LayoutElement.ignoreLayout)
                UpdateLayoutElementSize();
            else
                UpdateRectTransformSize();
        }

        protected virtual void UpdateRectTransformSize()
        {
            if (Height > Tolerance)
                RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, GetActualHeight());
            if (Width > Tolerance) 
                RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, GetActualWidth());
        }
        protected virtual void UpdateLayoutElementSize()
        {
            if (Height > Tolerance)
                LayoutElement.preferredHeight = GetActualHeight();
            if (Width > Tolerance)
                LayoutElement.preferredWidth = GetActualWidth();

            SetDirty((RectTransform)transform.parent);
        }

        protected float GetActualWidth() => Width * heightProportion;
        protected float GetActualHeight() => Height * heightProportion;

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
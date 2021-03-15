using UnityEngine;
using UnityEngine.EventSystems;

namespace ClinicalTools.UI
{
    [ExecuteAlways]
    public class AnchorsToMatchAspectRatio : UIBehaviour
    {
        public enum HorizontalAlignment { Left, Center, Right }
        public Vector2 AspectRatio { get => aspectRatio; set => aspectRatio = value; }
        [SerializeField] private Vector2 aspectRatio = Vector2.one;
        public float Offset { get => offset; set => offset = value; }
        [SerializeField] private float offset = 0;
        public HorizontalAlignment Alignment { get => alignment; set => alignment = value; }
        [SerializeField] private HorizontalAlignment alignment = 0;

        public float Width => RectTransform.anchorMax.x - RectTransform.anchorMin.x;

        protected RectTransform RectTransform => (RectTransform)transform;


        private bool dimensionsChanging;
        protected override void OnRectTransformDimensionsChange()
        {
            if (dimensionsChanging)
                return;

            dimensionsChanging = true;
            base.OnRectTransformDimensionsChange();
            UpdateAnchors();
            dimensionsChanging = false;
        }

        protected override void Start()
        {
            base.Start();
            UpdateAnchors();
        }

        private float lastOffset;
        private HorizontalAlignment lastAlignment;
        protected virtual void Update()
        {
            if (lastOffset != Offset || lastAlignment != Alignment)
                UpdateAnchors();
        }

        protected virtual void UpdateAnchors()
        {
            lastOffset = Offset;
            lastAlignment = Alignment;

            var parentRect = ((RectTransform)transform.parent).rect;
            var proportion = AspectRatio.x / AspectRatio.y;
            var width = parentRect.height * proportion;
            var proportionWidth = width / parentRect.width;

            if (proportionWidth > 0) {
                float leftBound = GetLeftBound(proportionWidth) + Offset;
                RectTransform.anchorMin = new Vector2(leftBound, 0);
                RectTransform.anchorMax = new Vector2(leftBound + proportionWidth, 1);
            } else {
                RectTransform.anchorMin = Vector2.zero;
                RectTransform.anchorMax = Vector2.one;
            }
        }

        protected virtual float GetLeftBound(float proportionWidth)
        {
            switch (Alignment) {
                case HorizontalAlignment.Left:
                    return 0;
                case HorizontalAlignment.Center:
                    return .5f - proportionWidth / 2f;
                case HorizontalAlignment.Right:
                    return 1f - proportionWidth;
                default:
                    return -1;
            }

        }
    }
}
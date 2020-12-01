using System;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.UI
{
    public class SetDimensionsBasedOnResolution : MonoBehaviour
    {
        public RectTransform Canvas { get => canvas; set => canvas = value; }
        [SerializeField] private RectTransform canvas;

        public Vector2 LandscapeDimensions { get => landscapeDimensions; set => landscapeDimensions = value; }
        [Tooltip("Dimensions in a 1920x1080 resolution")]
        [SerializeField] private Vector2 landscapeDimensions;
        public Vector2 PortraitDimensions { get => portraitDimensions; set => portraitDimensions = value; }
        [Tooltip("Dimensions in a 1080x1920 resolution")]
        [SerializeField] private Vector2 portraitDimensions;

        private readonly Vector2 LandscapeScreenDimensions = new Vector2(1920, 1080);
        private readonly Vector2 PortraitScreenDimensions = new Vector2(1080, 1920);

        private LayoutElement layoutElement;


        protected virtual void Awake()
        {
            layoutElement = GetComponent<LayoutElement>();
            UpdateSize();
        }
        protected virtual void Update() => UpdateSize();

        private Vector2 canvasSize = new Vector2();
        protected virtual void UpdateSize()
        {
            var canvasRect = Canvas.rect;
            var currentCanvasSize = new Vector2(canvasRect.width, canvasRect.height);
            if (currentCanvasSize == canvasSize)
                return;

            canvasSize = currentCanvasSize;

            var rectTrans = (RectTransform)transform;
            var sizeDelta = rectTrans.sizeDelta;
            if (ShouldSize(GetX)) {
                var width = GetSize(canvasSize, GetX, GetY);
                if (layoutElement != null)
                    layoutElement.preferredWidth = width;
                else
                    sizeDelta.x = width;
            }
            if (ShouldSize(GetY)) {
                var height = GetSize(canvasSize, GetY, GetX);
                if (layoutElement != null)
                    layoutElement.preferredHeight = height;
                else
                    sizeDelta.y = height;
            }
            rectTrans.sizeDelta = sizeDelta;
        }

        protected virtual float GetX(Vector2 vector) => vector.x;
        protected virtual float GetY(Vector2 vector) => vector.y;

        private const float Tolerance = .0001f;
        private bool ShouldSize(Func<Vector2, float> primaryValueGetter)
            => primaryValueGetter(LandscapeDimensions) > Tolerance || primaryValueGetter(PortraitDimensions) > Tolerance;


        protected virtual float GetSize(Vector2 dimensions, Func<Vector2, float> primaryValueGetter, Func<Vector2, float> secondaryValueGetter)
        {
            var slope = GetSlope(primaryValueGetter, secondaryValueGetter);
            var y1 = GetCanvasPercentage(PortraitDimensions, PortraitScreenDimensions, primaryValueGetter);
            var x1 = GetAspectRatioProportion(PortraitScreenDimensions, primaryValueGetter, secondaryValueGetter);
            var x = GetAspectRatioProportion(dimensions, primaryValueGetter, secondaryValueGetter);

            var proportion = Mathf.Clamp01(slope * (x - x1) + y1);
            return proportion * primaryValueGetter(dimensions);
        }
        protected virtual float GetSlope(Func<Vector2, float> primaryValueGetter, Func<Vector2, float> secondaryValueGetter)
        {
            var y1 = GetCanvasPercentage(PortraitDimensions, PortraitScreenDimensions, primaryValueGetter);
            var y2 = GetCanvasPercentage(LandscapeDimensions, LandscapeScreenDimensions, primaryValueGetter);
            var x1 = GetAspectRatioProportion(PortraitScreenDimensions, primaryValueGetter, secondaryValueGetter);
            var x2 = GetAspectRatioProportion(LandscapeScreenDimensions, primaryValueGetter, secondaryValueGetter);
            return (y1 - y2) / (x1 - x2);
        }

        protected virtual float GetCanvasPercentage(Vector2 dimensions, Vector2 screenDimension, Func<Vector2, float> primaryValueGetter)
            => primaryValueGetter(dimensions) / primaryValueGetter(screenDimension);
        protected virtual float GetAspectRatioProportion(Vector2 dimensions, Func<Vector2, float> primaryValueGetter, Func<Vector2, float> secondaryValueGetter)
            => secondaryValueGetter(dimensions) / primaryValueGetter(dimensions);
    }
}
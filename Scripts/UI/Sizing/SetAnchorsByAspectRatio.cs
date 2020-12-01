using UnityEngine;

namespace ClinicalTools.UI
{
    [ExecuteAlways]
    public class SetAnchorsByAspectRatio : MonoBehaviour
    {
        public Vector2 LandscapeMinAnchor { get => landscapeMinAnchor; set => landscapeMinAnchor = value; }
        [Tooltip("Minimum anchor in a 16:9 resolution")]
        [SerializeField] private Vector2 landscapeMinAnchor;
        public Vector2 LandscapeMaxAnchor { get => landscapeMaxAnchor; set => landscapeMaxAnchor = value; }
        [Tooltip("Maximum anchor in a 16:9 resolution")]
        [SerializeField] private Vector2 landscapeMaxAnchor;
        public Vector2 PortraitMinAnchor { get => portraitMinAnchor; set => portraitMinAnchor = value; }
        [Tooltip("Minimum anchor in a 9:16 resolution")]
        [SerializeField] private Vector2 portraitMinAnchor;
        public Vector2 PortraitMaxAnchor { get => portraitMaxAnchor; set => portraitMaxAnchor = value; }
        [Tooltip("Maximum anchor in a 9:16 resolution")]
        [SerializeField] private Vector2 portraitMaxAnchor;


        private const float LandscapeAspectRatio = 9f / 16;
        private const float PortraitAspectRatio = 16f / 9;

        protected virtual void Awake() => UpdateSize();

        protected virtual void Update() => UpdateSize();

        private const float Tolerance = .0001f;
        private Vector2Int canvasSize;

        private Vector2 lastLandscapeMinAnchor, lastLandscapeMaxAnchor, lastPortraitMinAnchor, lastPortraitMaxAnchor;
        protected virtual void UpdateSize()
        {
            var currentCanvasSize = new Vector2Int(Screen.width, Screen.height);
            if (currentCanvasSize != canvasSize) {
                canvasSize = currentCanvasSize;

                UpdateMinAnchor();
                UpdateMaxAnchor();
                return;
            }

            if (lastLandscapeMinAnchor != LandscapeMinAnchor || lastPortraitMinAnchor != PortraitMinAnchor)
                UpdateMinAnchor();
            if (lastLandscapeMaxAnchor != LandscapeMaxAnchor || lastPortraitMaxAnchor != PortraitMaxAnchor)
                UpdateMaxAnchor();

        }

        protected virtual void UpdateMinAnchor()
        {
            lastLandscapeMinAnchor = LandscapeMinAnchor;
            lastPortraitMinAnchor = PortraitMinAnchor;

            var rectTransform = (RectTransform)transform;
            SetAnchor(canvasSize, rectTransform.anchorMin, LandscapeMinAnchor, PortraitMinAnchor);
            rectTransform.offsetMin = Vector2.zero;
        }

        protected virtual void UpdateMaxAnchor()
        {
            lastLandscapeMaxAnchor = LandscapeMaxAnchor;
            lastPortraitMaxAnchor = PortraitMaxAnchor;

            var rectTransform = (RectTransform)transform;
            SetAnchor(canvasSize, rectTransform.anchorMax, LandscapeMaxAnchor, PortraitMaxAnchor);
            rectTransform.offsetMax = Vector2.zero;
        }

        protected virtual void SetAnchor(Vector2Int canvasSize, Vector2 currentAnchor, 
            Vector2 landscapeAnchor, Vector2 portraitAnchor)
        {
            if (canvasSize.x == 0)
                return;
            var aspectRatio = canvasSize.y / canvasSize.x;
            if (landscapeAnchor.x > Tolerance || portraitAnchor.x > Tolerance)
                currentAnchor.x = GetValue(aspectRatio, landscapeAnchor.x, portraitAnchor.x);
            if (landscapeAnchor.y > Tolerance || portraitAnchor.y > Tolerance)
                currentAnchor.y = GetValue(aspectRatio, landscapeAnchor.y, portraitAnchor.y);
        }


        protected virtual float GetValue(float currentAspectRatio, float landscapeValue, float portraitValue)
        {
            var slope = (portraitValue - landscapeValue)
                            / (PortraitAspectRatio - LandscapeAspectRatio);
            var y1 = landscapeValue;
            var x1 = LandscapeAspectRatio;
            var x = currentAspectRatio;

            return slope * (x - x1) + y1;
        }
    }
}
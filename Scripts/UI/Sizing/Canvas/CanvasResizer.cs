using UnityEngine;

namespace ClinicalTools.UI
{
    public class CanvasResizer : MonoBehaviour
    {
        protected static CanvasResizer Instance { get; set; }

        private static float resizeValue = .1f;
        public static float ResizeValue01 {
            get => resizeValue;
            set => resizeValue = Mathf.Clamp01(value);
        }
        public static float GetResizeValue() => 1f + .8f * ResizeValue01;

        protected void Awake() => Instance = this;


        private float startDistance;
        private float prevDistance = 0;
        private const float ZoomDeadzone = 50;
        private const float DistanceToMax = 300;
        bool zooming = true;
        private void Update()
        {
            if (Instance != this)
                return;

            var tapCount = Input.touchCount;
            if (tapCount <= 1) {
                zooming = false;
                return;
            }

            var touch1 = Input.GetTouch(0).position;
            var touch2 = Input.GetTouch(1).position;

            switch (Input.GetTouch(1).phase) {
                case TouchPhase.Began:
                    startDistance = Vector2.Distance(touch1, touch2);
                    break;
                case TouchPhase.Moved:
                    CalcZoomScale(touch1, touch2);
                    break;
            }
        }

        private void CalcZoomScale(Vector2 touch1, Vector2 touch2)
        {
            if (zooming)
                ContinueZooming(touch1, touch2);
            else if (Mathf.Abs(Vector2.Distance(touch1, touch2) - startDistance) >= ZoomDeadzone)
                StartZooming(touch1, touch2);
        }

        private void ContinueZooming(Vector2 touch1, Vector2 touch2)
        {
            float deltaTouch = Mathf.Abs(Vector2.Distance(touch1, touch2));
            float deltaDistance = deltaTouch - prevDistance;

            ResizeValue01 += deltaDistance / DistanceToMax;

            prevDistance = deltaTouch;
        }
        private void StartZooming(Vector2 touch1, Vector2 touch2)
        {
            float deltaTouch = Vector2.Distance(touch1, touch2) - startDistance;
            prevDistance = startDistance;
            if (deltaTouch > startDistance + ZoomDeadzone)
                prevDistance += ZoomDeadzone;
            else
                prevDistance -= ZoomDeadzone;

            zooming = true;
        }
    }
}

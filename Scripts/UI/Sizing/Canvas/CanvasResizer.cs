using System;
using UnityEngine;

namespace ClinicalTools.UI
{
    public class CanvasResizer : MonoBehaviour
    {
        public event Action<float> Resized;

        private const float MinValue = .925f;
        private const float MaxValue = 1.3f;
        private const float StartValue = 1.075f;

        public float ResizeValue { get; protected set; } = StartValue;

        private const float Scale = MaxValue - MinValue;

        private float resizeValue01 = (StartValue - MinValue) / Scale;
        public float ResizeValue01 {
            get => resizeValue01;
            set {
                resizeValue01 = Mathf.Clamp01(value);
                ResizeValue = MinValue + Scale * ResizeValue01;
                Resized?.Invoke(ResizeValue);
            }
        }

        private float startDistance;
        private float prevDistance = 0;
        private const float ZoomDeadzone = 50;
        private const float DistanceToMax = 300;
        bool zooming = true;
        private void Update()
        {
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

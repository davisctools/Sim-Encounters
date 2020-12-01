using System;
using UnityEngine;

namespace ClinicalTools.UI
{
    public class SwipeHandler
    {
        public event Action SwipeLeft;
        public event Action SwipeRight;

        // Amount angle can be from straight to count as a swipe
        private const float ANGLE_TOLERANCE = 15f;
        private const float MIN_HORIZONTAL_DIST = 100f;
        private const float MAX_TIME = 5f;

        public bool IsSwiping { get; private set; }
        private Vector2 startPosition;
        private Vector2 endPosition;
        private float startTime;


        public void TouchPosition(Vector2 newPos)
        {
            if (!IsSwiping) {
                IsSwiping = true;
                startPosition = newPos;
                startTime = Time.time;
            }

            endPosition = newPos;
        }

        public void ReleaseTouch()
        {
            IsSwiping = false;

            if (Time.time - startTime > MAX_TIME)
                return;

            if (Mathf.Abs(startPosition.x - endPosition.x) > MIN_HORIZONTAL_DIST) {
                var angle = Vector2.Angle(Vector2.left, startPosition - endPosition);
                if (angle < ANGLE_TOLERANCE) {
                    SwipeRight?.Invoke();
                } else if (180 - angle < ANGLE_TOLERANCE) {
                    SwipeLeft?.Invoke();
                }
            }
        }
    }
}
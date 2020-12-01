using UnityEngine;

namespace ClinicalTools.UI
{
    public class Swipe
    {
        public Vector2 StartPosition { get; }

        private float lastTime;
        private Vector2 lastPosition;
        public Vector2 LastPosition {
            get => lastPosition;
            set {
                var currentTime = Time.realtimeSinceStartup;
                if (lastPosition != default)
                    Velocity = (value - lastPosition) / (currentTime - lastTime);

                lastTime = currentTime;
                lastPosition = value;
            }
        }
        public Vector2 Velocity { get; protected set; }

        public bool Ended { get; set; }

        public float InitialAngle { get; }

        public Swipe(Vector2 startPosition)
        {
            StartPosition = startPosition;
            LastPosition = startPosition;
        }
        public Swipe(Vector2 startPosition, Vector2 endPosition)
        {
            StartPosition = startPosition;
            LastPosition = endPosition;
            InitialAngle = Vector2.Angle(Vector2.left, StartPosition - LastPosition);
            if (LastPosition.y < StartPosition.y)
                InitialAngle = 360 - InitialAngle;
        }
    }
}

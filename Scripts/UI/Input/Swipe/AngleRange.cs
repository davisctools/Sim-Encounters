namespace ClinicalTools.UI
{
    /// <summary>
    /// Angles making up the range, with the angles being listed in counter-clockwise order.
    /// </summary>
    public class AngleRange
    {
        private const float DegreesInCircle = 360;

        private float start;
        public float Start {
            get => start;
            set => start = GetAngleValue(value);
        }
        private float end;
        public float End {
            get => end;
            set => end = GetAngleValue(value);
        }

        public AngleRange(float start, float end)
        {
            Start = start;
            End = end;
        }

        protected float GetAngleValue(float angle)
        {
            angle %= DegreesInCircle;
            if (angle < 0)
                angle += DegreesInCircle;
            return angle;
        }

        public bool ContainsAngle(float angle)
        {
            angle = GetAngleValue(angle);

            if (End > Start)
                return angle >= Start && angle <= End;
            else
                return angle >= Start || angle <= End;
        }

        public bool ContainsAngleRange(AngleRange angleRange)
        {
            if (End > Start) {
                if (angleRange.Start < Start || angleRange.Start > End)
                    return false;

                return angleRange.End >= angleRange.Start && angleRange.Start <= End;
            }

            if (angleRange.Start >= Start)
                return angleRange.End >= angleRange.Start || angleRange.End <= End;

            if (angleRange.Start <= End)
                return angleRange.End >= angleRange.Start && angleRange.End <= End;

            return false;
        }

        /// <summary>
        /// Adds an angle range to this angle range. Either the start or the end must be within the angle range.
        /// </summary>
        /// <param name="angleRange">Angle range to add</param>
        public void AddAngleRange(AngleRange angleRange)
        {
            var containsStart = ContainsAngle(angleRange.Start);
            var containsEnd = ContainsAngle(angleRange.End);
            if (containsStart == containsEnd)
                return;

            if (containsStart)
                End = angleRange.end;
            else
                Start = angleRange.Start;
        }

        public float GetRangeDistance() => GetAngleValue(End - Start);
        public void FlipStartAndEnd()
        {
            var oldStart = Start;
            Start = End;
            End = oldStart;
        }
    }
}

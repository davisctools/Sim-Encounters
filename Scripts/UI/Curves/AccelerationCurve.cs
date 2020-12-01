using UnityEngine;

namespace ClinicalTools.UI
{
    public class AccelerationCurve : ICurve
    {
        private const float InitialVelocity = 1.5f;
        private const float Acceleration = 1 - InitialVelocity;
        public virtual float GetCurveY(float curveX)
            => Mathf.Clamp01((Acceleration * Mathf.Clamp01(curveX) + InitialVelocity) * Mathf.Clamp01(curveX));
        public virtual float GetCurveX(float curveY)
        {
            var discriminant = InitialVelocity * InitialVelocity + 4 * Acceleration * Mathf.Clamp01(curveY);
            return (-InitialVelocity + Mathf.Sqrt(discriminant)) / (2 * Acceleration);
        }
    }
}
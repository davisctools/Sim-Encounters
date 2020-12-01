using UnityEngine;

namespace ClinicalTools.UI
{
    public class ExponentialCurve : ICurve
    {
        private const float Exp = 1.25f;
        public virtual float GetCurveY(float curveX) => Mathf.Clamp01(1 - Mathf.Pow(1 - Mathf.Clamp01(curveX), Exp));
        public virtual float GetCurveX(float curveY) => Mathf.Clamp01(1 - Mathf.Pow(1 - Mathf.Clamp01(curveY), 1 / Exp));
    }
}
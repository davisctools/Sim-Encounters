namespace ClinicalTools.UI
{
    public interface ICurve
    {
        float GetCurveY(float curveX);
        float GetCurveX(float curveY);
    }
}
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.UI
{
    public class BetterCanvasScaler : CanvasScaler
    {
        // Some things only work with ints (like layout group padding),
        // so small width and height numbers don't allow for the precision sometimes needed
        public static float Scalar { get; set; } = 1.1f;
        protected override void HandleConstantPhysicalSize()
        {
            var editorDPI = m_FallbackScreenDPI * Screen.height / 2500 * 1.08f;
            float currentDpi = editorDPI;

            //currentDpi = 96;
            float dpi = (currentDpi == 0 ? m_FallbackScreenDPI : currentDpi);
            float targetDPI = 1;
            switch (m_PhysicalUnit) {
                case Unit.Centimeters: targetDPI = 2.54f; break;
                case Unit.Millimeters: targetDPI = 25.4f; break;
                case Unit.Inches: targetDPI = 1; break;
                case Unit.Points: targetDPI = 72; break;
                case Unit.Picas: targetDPI = 6; break;
            }

            var spriteDpiProportion = dpi / m_DefaultSpriteDPI;

            SetScaleFactor(dpi / targetDPI);
            SetReferencePixelsPerUnit(m_ReferencePixelsPerUnit * targetDPI / m_DefaultSpriteDPI);
        }
    }
}

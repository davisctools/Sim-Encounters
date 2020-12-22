using ClinicalTools.SEColors;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class FeedbackColorManager : IFeedbackColorManager
    {
        protected virtual Color DefaultColor { get; } = new Color(0.9372549f, 0.9372549f, 0.9372549f, 1f);

        protected virtual IColorManager ColorManager { get; } = new ColorManager();

        public virtual Color GetColor(OptionType optionType)
        {
            switch (optionType) {
                case OptionType.Correct:
                    return ColorManager.GetColor(ColorType.LightCorrect);
                case OptionType.Incorrect:
                    return ColorManager.GetColor(ColorType.LightIncorrect);
                case OptionType.PartiallyCorrect:
                    return ColorManager.GetColor(ColorType.LightPartiallyCorrect);
                default:
                    return DefaultColor;
            }
        }

        public virtual Color GetDefaultColor() => DefaultColor;
    }
}
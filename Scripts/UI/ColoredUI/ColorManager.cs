using UnityEngine;

namespace ClinicalTools.UI
{
    // while assigning each value of an enum to a number is usually frowned upon,
    // this allows new values to be added anywhere in the enum wihtout messing up serialized uses
    public enum ColorType
    {
        PrimaryColor = 0,
        DarkColor = 1,
        LightColor = 2,
        OffBlack = 3, 
        Green = 4,

        Gray1 = 5,
        Gray2 = 10,
        Gray3 = 11,
        Gray4 = 6,
        Gray5 = 7,
        Gray6 = 8,
        Gray7 = 9,

        Correct = 12,
        PartiallyCorrect = 13,
        Incorrect = 14,
        LightCorrect = 15,
        LightPartiallyCorrect = 16,
        LightIncorrect = 17,
    }
    // I don't like using white, because it is used a lot, 
    // and I can't imagine all instances changing at once
    // If an offwhite is eventually needed, I can see more usage
    public interface IColorManager
    {
        Color GetColor(ColorType colorType);
    }

    public class ColorManager : IColorManager
    {
        private static readonly Color primaryColor = new Color(0.2784314f, 0.4666667f, 0.6980392f);
        private static readonly Color darkColor = new Color(0.1960784f, 0.3529412f, 0.5529412f);
        private static readonly Color lightColor = new Color(0.4150943f, 0.601305f, 0.8301887f);
        private static readonly Color offBlackColor = new Color(0.1490196f, 0.1882353f, 0.2196078f);
        private static readonly Color greenColor = new Color(0.3607843f, 0.7215686f, 0.3607843f);
      
        private static readonly Color gray1Color = new Color(0.937f, 0.953f, 0.965f);
        private static readonly Color gray2Color = new Color(0.918f, 0.933f, 0.945f);
        private static readonly Color gray3Color = new Color(0.847f, 0.875f, 0.898f);
        private static readonly Color gray4Color = new Color(0.780f, 0.816f, 0.851f);
        private static readonly Color gray5Color = new Color(0.675f, 0.737f, 0.788f);
        private static readonly Color gray6Color = new Color(0.522f, 0.592f, 0.647f);
        private static readonly Color gray7Color = new Color(0.482f, 0.486f, 0.494f);

        private static readonly Color lightIncorrectColor = new Color(1f, 0.8588235f, 0.8588235f);
        private static readonly Color lightPartiallyCorrectColor = new Color(0.9607843f, 0.9333333f, 0.8235294f);
        private static readonly Color lightCorrectColor = new Color(0.8627451f, 0.9372549f, 0.8588235f);
        private static readonly Color incorrectColor = new Color(0.8470588f, 0.2352941f, 0.2352941f, 0.6f);
        private static readonly Color partiallyCorrectColor = new Color(1, 0.7019608f, 0.2862745f, 0.6f);
        private static readonly Color correctColor = new Color(0.2627451f, 0.7294118f, 0.282353f, 0.6f);


        public Color GetColor(ColorType colorType)
        {
            switch (colorType) {
                case ColorType.PrimaryColor:
                    return primaryColor;
                case ColorType.DarkColor:
                    return darkColor;
                case ColorType.LightColor:
                    return lightColor;
                case ColorType.OffBlack:
                    return offBlackColor;
                case ColorType.Green:
                    return greenColor;
                case ColorType.Gray1:
                    return gray1Color;
                case ColorType.Gray2:
                    return gray2Color;
                case ColorType.Gray3:
                    return gray3Color;
                case ColorType.Gray4:
                    return gray4Color;
                case ColorType.Gray5:
                    return gray5Color;
                case ColorType.Gray6:
                    return gray6Color;
                case ColorType.Gray7:
                    return gray7Color;
                case ColorType.Correct:
                    return correctColor;
                case ColorType.PartiallyCorrect:
                    return partiallyCorrectColor;
                case ColorType.Incorrect:
                    return incorrectColor;
                case ColorType.LightCorrect:
                    return lightCorrectColor;
                case ColorType.LightPartiallyCorrect:
                    return lightPartiallyCorrectColor;
                case ColorType.LightIncorrect:
                    return lightIncorrectColor;
                default:
                    return primaryColor;
            }
        }
    }
}
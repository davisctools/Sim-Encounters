using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class ColorDeserializer : IStringDeserializer<Color>
    {
        private const string ColorPrefix = "RGBA(";
        private const string ColorSuffix = ")";
        public Color Deserialize(string colorString)
        {
            if (colorString == null || !colorString.StartsWith(ColorPrefix) || !colorString.EndsWith(ColorSuffix))
                return Color.clear;

            var colorLength = colorString.Length - ColorPrefix.Length - ColorSuffix.Length;
            colorString = colorString.Substring(ColorPrefix.Length, colorLength);

            var colorParts = colorString.Split(',');
            if (colorParts.Length != 4)
                return Color.clear;
            if (float.TryParse(colorParts[0].Trim(), out var red) && float.TryParse(colorParts[1].Trim(), out var green) &&
                float.TryParse(colorParts[2].Trim(), out var blue) && float.TryParse(colorParts[3].Trim(), out var alpha)) {

                return new Color(red, green, blue, alpha);
            }

            return Color.clear;
        }

    }
}
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class Icon
    {
        public enum IconType { Resource, Upload, EncounterImage }
        public IconType Type { get; }
        public string Reference { get; }
        public Color Color { get; set; } = Color.white;

        public Icon() => Type = IconType.EncounterImage;
        public Icon(IconType type, string reference)
        {
            Type = type;
            Reference = reference;
        }
        public Icon(IconType type, string reference, Color color)
        {
            Type = type;
            Reference = reference;
            Color = color;
        }
    }
}
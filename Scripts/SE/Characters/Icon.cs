namespace ClinicalTools.SimEncounters
{
    public class Icon
    {
        public enum IconType { Resource, Upload, EncounterImage }
        public IconType Type { get; }
        public string Reference { get; }

        public Icon() => Type = IconType.EncounterImage;
        public Icon(IconType type, string reference)
        {
            Type = type;
            Reference = reference;
        }
    }
}
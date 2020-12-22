using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class Character
    {
        public Color PrimaryColor { get; set; } = new Color(0.937f, 0.953f, 0.965f);
        public Color SecondaryColor { get; set; } = new Color(0.937f, 0.953f, 0.965f);
        public CharacterColorTheme ColorTheme { get; set; } = new CharacterColorTheme();
        public Icon Icon { get; set; }
        public string Role { get; set; } = "";
        public string Name { get; set; } = "";
    }

    public class CharacterColorTheme
    {
        public Color IconBackgroundColor { get; set; } = new Color(0.937f, 0.953f, 0.965f);
        public Color DialogueBackgroundColor { get; set; } = new Color(0.937f, 0.953f, 0.965f);
    }
}
using ClinicalTools.SEColors;

namespace ClinicalTools.SimEncounters
{
    public class Character
    {
        public CharacterColorTheme ColorTheme { get; set; } = new CharacterColorTheme();
        public Icon Icon { get; set; }
        public string Role { get; set; } = "";
        public string Name { get; set; } = "";
    }
}
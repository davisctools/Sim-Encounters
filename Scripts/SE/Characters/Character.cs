using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class Character
    {
        public Color PrimaryColor { get; set; } = Color.white;
        public Color SecondaryColor { get; set; } = Color.white;
        public Icon Icon { get; set; }
        public string Role { get; set; } = "";
        public string Name { get; set; } = "";
    }
}
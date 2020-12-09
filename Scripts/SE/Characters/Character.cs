using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class Character
    {
        public Color Color { get; set; } = Color.white;
        public Icon Icon { get; set; }
        public string Role { get; set; } = "";
        public string Name { get; set; } = "";
    }
}
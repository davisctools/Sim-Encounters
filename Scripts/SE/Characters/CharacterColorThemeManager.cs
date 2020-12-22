using UnityEngine;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public class CharacterColorThemeManager
    {
        protected virtual List<CharacterColorTheme> ColorThemes { get; }
            = new List<CharacterColorTheme>() {
                new CharacterColorTheme(),
                new CharacterColorTheme(),
                new CharacterColorTheme(),
                new CharacterColorTheme(),
                new CharacterColorTheme(),
                new CharacterColorTheme(),
                new CharacterColorTheme(),
                new CharacterColorTheme(),
            };

        public CharacterColorTheme GetColorTheme(Color color)
        {
            return ColorThemes[0];
        }
        public CharacterColorTheme GetColorTheme(int number)
        {
            if (number < 0 || number >= ColorThemes.Count)
                return ColorThemes[0];
            return ColorThemes[number];
        }

        public int GetThemeIndex(CharacterColorTheme colorTheme) => ColorThemes.IndexOf(colorTheme);
    }

}
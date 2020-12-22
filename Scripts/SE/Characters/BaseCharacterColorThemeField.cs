using System;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseCharacterColorThemeField : MonoBehaviour
    {
        public abstract event Action<CharacterColorTheme> ValueChanged;

        public abstract void Display(CharacterColorTheme colorTheme);
        public abstract CharacterColorTheme GetValue();
    }
}
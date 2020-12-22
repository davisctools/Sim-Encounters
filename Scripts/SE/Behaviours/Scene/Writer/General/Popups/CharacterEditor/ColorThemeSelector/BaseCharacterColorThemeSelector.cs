using ClinicalTools.SEColors;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseCharacterColorThemeSelector : MonoBehaviour
    {
        public abstract WaitableTask<CharacterColorTheme> SelectColorTheme(CharacterColorTheme colorTheme);
    }
}
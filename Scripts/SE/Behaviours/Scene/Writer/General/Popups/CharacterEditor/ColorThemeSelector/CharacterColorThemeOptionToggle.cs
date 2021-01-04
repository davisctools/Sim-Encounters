using UnityEngine;
using UnityEngine.UI;
using System;
using ClinicalTools.SEColors;

namespace ClinicalTools.SimEncounters
{
    [ExecuteAlways]
    [RequireComponent(typeof(Toggle))]
    public class CharacterColorThemeOptionToggle : MonoBehaviour
    {
        [SerializeField] private ColorThemeName colorThemeIndex;

        public event Action<CharacterColorTheme> Selected;

        protected virtual ICharacterColorThemeManager CharacterColorThemeManager { get; } = new CharacterColorThemeManager();

        public CharacterColorTheme ColorTheme 
            => colorTheme ?? (colorTheme = CharacterColorThemeManager.GetColorTheme(colorThemeIndex));
        private CharacterColorTheme colorTheme;

        protected Toggle Toggle => (toggle == null) ? toggle = GetComponent<Toggle>() : toggle;
        private Toggle toggle;

        protected virtual void Awake() => Toggle.onValueChanged.AddListener(OnValueChanged);

        ColorThemeName lastColorThemeName = (ColorThemeName)(-1);
        protected virtual void Update()
        {
            if (lastColorThemeName == colorThemeIndex)
                return;

            lastColorThemeName = colorThemeIndex;
            colorTheme = CharacterColorThemeManager.GetColorTheme(colorThemeIndex);
            Toggle.image.color = ColorTheme.IconBackgroundColor;
        }

        protected virtual void OnValueChanged(bool isOn)
        {
            if (isOn)
                Selected?.Invoke(ColorTheme);
        }

        public virtual void Select() => Toggle.isOn = true;
    }

}
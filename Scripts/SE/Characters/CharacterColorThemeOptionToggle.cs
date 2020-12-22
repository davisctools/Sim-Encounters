using UnityEngine;
using UnityEngine.UI;
using Zenject;
using System;

namespace ClinicalTools.SimEncounters
{
    [RequireComponent(typeof(Toggle))]
    public class CharacterColorThemeOptionToggle : MonoBehaviour
    {
        [SerializeField] private int colorThemeIndex;

        public event Action<CharacterColorTheme> Selected;

        protected CharacterColorThemeManager CharacterColorThemeManager { get; set; }
        [Inject]
        public virtual void Inject(CharacterColorThemeManager characterColorThemeManager)
            => CharacterColorThemeManager = characterColorThemeManager;

        private CharacterColorTheme colorTheme;
        public CharacterColorTheme ColorTheme {
            get {
                if (colorTheme == null)
                    colorTheme = CharacterColorThemeManager.GetColorTheme(colorThemeIndex);
                return colorTheme;
            }
        }
        private Toggle toggle;
        protected Toggle Toggle {
            get {
                if (toggle == null)
                    toggle = GetComponent<Toggle>();
                return toggle;
            }
        }
        protected virtual void Awake()=> Toggle.onValueChanged.AddListener(OnValueChanged);
        protected virtual void OnValueChanged(bool isOn)
        {
            if (isOn)
                Selected?.Invoke(ColorTheme);
        }

        public virtual void Select() => Toggle.isOn = true;
    }

}
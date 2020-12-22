using ClinicalTools.SEColors;
using ClinicalTools.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    public class CharacterColorThemeSelectorPopup : BaseCharacterColorThemeSelector, ICloseHandler
    {
        [SerializeField] private Button applyButton;

        protected Dictionary<CharacterColorTheme, CharacterColorThemeOptionToggle> ColorThemeOptions { get; } 
            = new Dictionary<CharacterColorTheme, CharacterColorThemeOptionToggle>();
        protected virtual void Awake()
        {
            var options = GetComponentsInChildren<CharacterColorThemeOptionToggle>();
            foreach (var option in options) {
                ColorThemeOptions.Add(option.ColorTheme, option);
                option.Selected += OnOptionSelected;
            }
            applyButton.onClick.AddListener(Apply);
        }

        protected virtual void OnOptionSelected(CharacterColorTheme colorTheme) => CurrentColorTheme = colorTheme;

        protected CharacterColorTheme CurrentColorTheme { get; set; }
        protected WaitableTask<CharacterColorTheme> CurrentColorThemeTask { get; set; }
        public override WaitableTask<CharacterColorTheme> SelectColorTheme(CharacterColorTheme colorTheme)
        {
            CurrentColorThemeTask?.SetError(new Exception("New popup opened"));
            CurrentColorThemeTask = new WaitableTask<CharacterColorTheme>();

            gameObject.SetActive(true);

            if (ColorThemeOptions.ContainsKey(colorTheme))
                ColorThemeOptions[colorTheme].Select();

            CurrentColorTheme = colorTheme;

            return CurrentColorThemeTask;
        }

        protected virtual void Apply()
        {
            CurrentColorThemeTask.SetResult(CurrentColorTheme);
            CurrentColorThemeTask = null;
            gameObject.SetActive(false);
        }
        public virtual void Close(object sender)
        {
            CurrentColorThemeTask.SetError(new Exception("Could not set result."));
            CurrentColorThemeTask = null;
            gameObject.SetActive(false);
        }
    }
}
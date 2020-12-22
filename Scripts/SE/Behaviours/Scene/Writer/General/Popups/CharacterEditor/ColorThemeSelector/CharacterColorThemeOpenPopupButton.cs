using ClinicalTools.SEColors;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    [RequireComponent(typeof(Button))]
    public class CharacterColorThemeOpenPopupButton : BaseCharacterColorThemeField
    {
        [SerializeField] private Image image;

        public override event Action<CharacterColorTheme> ValueChanged;

        protected CharacterColorTheme ColorTheme { get; set; }

        protected ISelectedListener<EncounterSelectedEventArgs> EncounterSelectedListener { get; set; }
        protected BaseCharacterColorThemeSelector ColorThemeSelectionPopup { get; set; }
        [Inject]
        public virtual void Inject(
            ISelectedListener<EncounterSelectedEventArgs> encounterSelectedListener,
            BaseCharacterColorThemeSelector colorThemeSelectionPopup)
        {
            EncounterSelectedListener = encounterSelectedListener;
            ColorThemeSelectionPopup = colorThemeSelectionPopup;
        }

        protected virtual void Start() => GetComponent<Button>().onClick.AddListener(OnButtonClicked);
        protected virtual void OnButtonClicked()
            => ColorThemeSelectionPopup.SelectColorTheme(ColorTheme).AddOnCompletedListener(OnColorThemeSelected);

        protected virtual void OnColorThemeSelected(TaskResult<CharacterColorTheme> colorThemeResult)
        {
            if (colorThemeResult.HasValue())
                SetIcon(colorThemeResult.Value);
        }

        public override void Display(CharacterColorTheme colorTheme) => SetIcon(colorTheme);
        protected virtual void SetIcon(CharacterColorTheme colorTheme)
        {
            ColorTheme = colorTheme;
            ValueChanged?.Invoke(colorTheme);
            if (colorTheme != null)
                image.color = colorTheme.IconBackgroundColor;
        }

        public override CharacterColorTheme GetValue() => ColorTheme;
    }
}
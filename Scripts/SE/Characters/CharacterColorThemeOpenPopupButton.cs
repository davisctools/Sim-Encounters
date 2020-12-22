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
        protected virtual void OnButtonClicked() => ColorThemeSelectionPopup.SelectColorTheme(ColorTheme).AddOnCompletedListener(OnIconSelected);

        protected virtual void OnIconSelected(TaskResult<CharacterColorTheme> iconResult)
        {
            if (iconResult.HasValue())
                SetIcon(iconResult.Value);
        }

        public override void Display(CharacterColorTheme icon) => SetIcon(icon);
        protected virtual void SetIcon(CharacterColorTheme icon)
        {
            ColorTheme = icon;
            ValueChanged?.Invoke(icon);
            if (icon != null)
                image.color = icon.IconBackgroundColor;
        }

        public override CharacterColorTheme GetValue() => ColorTheme;
    }
}
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    [RequireComponent(typeof(Icon))]
    public class IconSelectionPopupButton : BaseIconSelector
    {
        [SerializeField] private Image image;

        public override event Action<Icon> ValueChanged;

        protected Icon Icon { get; set; }

        protected ISelectedListener<EncounterSelectedEventArgs> EncounterSelectedListener { get; set; }
        protected IconSpriteRetriever IconSpriteRetriever { get; set; }
        protected IconSelectorPopup IconSelectionPopup { get; set; }
        [Inject]
        public virtual void Inject(
            ISelectedListener<EncounterSelectedEventArgs> encounterSelectedListener,
            IconSpriteRetriever iconSpriteRetriever, 
            IconSelectorPopup colorSelectionPopup)
        {
            EncounterSelectedListener = encounterSelectedListener;
            IconSpriteRetriever = iconSpriteRetriever;
            IconSelectionPopup = colorSelectionPopup;
        }

        protected virtual void Start() => GetComponent<Button>().onClick.AddListener(OnButtonClicked);
        protected virtual void OnButtonClicked() => IconSelectionPopup.SelectIcon(Icon).AddOnCompletedListener(OnIconSelected);

        protected virtual void OnIconSelected(TaskResult<Icon> iconResult)
        {
            if (iconResult.HasValue())
                SetIcon(iconResult.Value);
        }

        public override void Display(Icon icon) => SetIcon(icon);
        protected virtual void SetIcon(Icon icon)
        {
            Icon = icon;
            ValueChanged?.Invoke(icon);
            image.sprite = IconSpriteRetriever.GetIconSprite(EncounterSelectedListener.CurrentValue.Encounter, icon);
        }

        public override Icon GetValue() => Icon;
    }
}
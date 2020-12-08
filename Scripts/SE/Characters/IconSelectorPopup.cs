using ClinicalTools.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class IconSelectorPopup : MonoBehaviour
    {
        [SerializeField] private IconSelectorToggle customIconToggle;
        [SerializeField] private Toggle useEncounterImageToggle;
        [SerializeField] private ToggleGroup toggleGroup;
        [SerializeField] private Button selectCustomImageButton;
        [SerializeField] private Button applyButton;

        protected IKeyedSpriteSelector SpriteSelector { get; set; }
        protected ISelectedListener<EncounterSelectedEventArgs> EncounterSelectedListener { get; set; }
        [Inject]
        public virtual void Inject(
            IKeyedSpriteSelector spriteSelector,
            ISelectedListener<EncounterSelectedEventArgs> encounterSelectedListener)
        {
            SpriteSelector = spriteSelector;
            EncounterSelectedListener = encounterSelectedListener;
        }

        protected Encounter Encounter => EncounterSelectedListener.CurrentValue.Encounter;
        protected KeyedCollection<Sprite> Sprites => Encounter.Content.ImageContent.Sprites;

        protected Dictionary<string, IconSelectorToggle> ResourceToggles { get; } = new Dictionary<string, IconSelectorToggle>();
        protected virtual void Awake()
        {
            var iconSelectors = GetComponentsInChildren<IconSelectorToggle>();
            foreach (var iconSelector in iconSelectors) {
                iconSelector.Selected += OnIconSelected;
                iconSelector.SetToggleGroup(toggleGroup);
                if (iconSelector.Icon?.Type == Icon.IconType.Resource)
                    ResourceToggles.Add(iconSelector.Icon.Reference, iconSelector);
            }

            useEncounterImageToggle.onValueChanged.AddListener(OnUseEncounterImageToggleValueChanged);
            selectCustomImageButton.onClick.AddListener(OnSelectCustomImageButtonPressed);
            applyButton.onClick.AddListener(Apply);
        }


        protected Icon CurrentIcon { get; set; }
        protected virtual void OnIconSelected(Icon icon) => CurrentIcon = icon;

        protected Icon LastUploadedIcon { get; set; }
        protected WaitableTask<Icon> CurrentIconTask { get; set; }
        public virtual WaitableTask<Icon> SelectIcon(Icon currentIcon)
        {
            CurrentIconTask?.SetError(new Exception("New popup opened"));
            CurrentIconTask = new WaitableTask<Icon>();

            gameObject.SetActive(true);

            CurrentIcon = currentIcon;
            if (currentIcon == null || currentIcon.Type == Icon.IconType.EncounterImage) {
                SetCurrentToEncounterImage();
            } else if (currentIcon.Type == Icon.IconType.Upload) {
                LastUploadedIcon = currentIcon;
                SetCurrentToLastUploadedIcon();
            } else if (currentIcon.Type == Icon.IconType.Resource) {
                var reference = currentIcon.Reference;
                if (ResourceToggles.ContainsKey(reference))
                    ResourceToggles[reference].Select();
                else
                    Debug.LogWarning($"Resource toggles does not contain a toggle with given reference ({reference}).");
            }

            return CurrentIconTask;
        }

        protected virtual void OnUseEncounterImageToggleValueChanged(bool isOn)
        {
            if (isOn)
                SetCurrentToEncounterImage();
            else
                SetCurrentToLastUploadedIcon();
        }

        protected virtual void SetCurrentToEncounterImage()
        {
            useEncounterImageToggle.SetIsOnWithoutNotify(true);
            CurrentIcon = new Icon();
            customIconToggle.SetIcon(CurrentIcon, Encounter.Metadata.Sprite);
            customIconToggle.Select();
        }
        protected virtual void SetCurrentToLastUploadedIcon()
        {
            useEncounterImageToggle.SetIsOnWithoutNotify(false);
            CurrentIcon = LastUploadedIcon;
            Sprite sprite;
            var reference = LastUploadedIcon?.Reference;
            sprite = reference != null && Sprites.ContainsKey(reference) ? Sprites[reference] : null;
            customIconToggle.SetIcon(CurrentIcon, sprite);
            customIconToggle.Select();
        }

        protected virtual void OnSelectCustomImageButtonPressed()
        {
            var spriteSelectedTask = SpriteSelector.SelectSprite(Sprites, LastUploadedIcon?.Reference);
            spriteSelectedTask.AddOnCompletedListener(OnCustomSpriteSelected);
        }

        protected virtual void OnCustomSpriteSelected(TaskResult<string> spriteKey)
        {
            if (spriteKey.IsError())
                return;

            LastUploadedIcon = spriteKey != null ? new Icon(Icon.IconType.Upload, spriteKey.Value) : null;
            SetCurrentToLastUploadedIcon();
        }

        protected virtual void Apply()
        {
            CurrentIconTask.SetResult(CurrentIcon);
            gameObject.SetActive(false);
        }
        protected virtual void Cancel()
        {
            CurrentIconTask.SetError(new Exception("Could not set result."));
            gameObject.SetActive(false);
        }
    }
}
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    public class EncounterImageOption : BaseEncounterImageOption
    {
        public override event Action<EncounterImage> ImageSelected;
        public override event Action<EncounterImage> ImageDeselected;

        [SerializeField] private Toggle toggle;
        [SerializeField] private Image image;

        protected virtual void Awake()
        {
            toggle.onValueChanged.AddListener(OnToggleChanged);
        }
        protected virtual void OnToggleChanged(bool isOn)
        {
            if (CurrentImage == null)
                return;

            var action = isOn ? ImageSelected : ImageDeselected;
            action?.Invoke(CurrentImage);
        }

        protected EncounterImage CurrentImage { get; set; }
        public override void Initialize(EncounterImage image)
        {
            CurrentImage = image;
            this.image.sprite = image.Sprite;
        }

        protected virtual void OnEnable() 
            => toggle.onValueChanged.AddListener(OnToggleChanged);
        protected virtual void OnDisable()
            => toggle.onValueChanged.RemoveListener(OnToggleChanged);

        public override void SetGroup(ToggleGroup group) => toggle.group = group;
        public override void Select()
        {
            if (toggle.isOn)
                ImageSelected?.Invoke(CurrentImage);
            else
                toggle.isOn = true;
        }

        public override void Deselect()
        {
            if (!toggle.isOn)
                ImageSelected?.Invoke(CurrentImage);
            else
                toggle.isOn = false;
        }
    }
}
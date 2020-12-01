using System;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.UI
{
    public class SelectableToggle : MonoBehaviour
    {
        public event Action Selected;
        public event Action Unselected;

        public Toggle Toggle { get => toggle; set => toggle = value; }
        [SerializeField] private Toggle toggle;

        protected virtual void Start() => Initialize();
        private bool initialized = false;
        protected virtual void Initialize()
        {
            if (initialized)
                return;
            initialized = true;
            toggle.onValueChanged.AddListener(ToggleChanged);
        }

        public virtual void Select()
        {
            Initialize();
            // changing to selected color should be instantaneous here, so fade duration needs to be temporarily changed
            var fadeDuration = Toggle.colors.fadeDuration;
            SetToggleFadeDuration(0);
            Toggle.isOn = true;
            SetToggleFadeDuration(fadeDuration);
        }

        public virtual void SelectWithNoNotify()
        {
            Initialize();
            // changing to selected color should be instantaneous here, so fade duration needs to be temporarily changed
            var fadeDuration = Toggle.colors.fadeDuration;
            SetToggleFadeDuration(0);
            Toggle.SetIsOnWithoutNotify(true);
            Toggle.interactable = false;
            SetToggleFadeDuration(fadeDuration);
        }
        public virtual void DeselectWithNoNotify()
        {
            Initialize();
            // changing to selected color should be instantaneous here, so fade duration needs to be temporarily changed
            var fadeDuration = Toggle.colors.fadeDuration;
            SetToggleFadeDuration(0);
            Toggle.SetIsOnWithoutNotify(false);
            Toggle.interactable = true;
            SetToggleFadeDuration(fadeDuration);
        }

        public void SetToggleGroup(ToggleGroup group) => Toggle.group = group;

        protected virtual void ToggleChanged(bool isOn)
        {
            Toggle.interactable = !isOn;
            if (isOn)
                Selected?.Invoke();
            else
                Unselected?.Invoke();
        }
        protected virtual void SetToggleFadeDuration(float fadeDuration)
        {
            var colorGroup = Toggle.colors;
            colorGroup.fadeDuration = fadeDuration;
            Toggle.colors = colorGroup;
        }
    }
}
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    public class IconSelectorToggle : MonoBehaviour
    {
        [SerializeField] private Image iconDisplay;
        [SerializeField] private Toggle toggle;
        public virtual Icon Icon { get; protected set; }

        public event Action<Icon> Selected;

        protected virtual void Start() => toggle.onValueChanged.AddListener(OnToggleChanged);


        protected virtual void OnToggleChanged(bool value)
        {
            if (value) Selected?.Invoke(Icon);
        }

        public virtual void SetIcon(Icon icon, Sprite sprite)
        {
            Icon = icon;
            iconDisplay.sprite = sprite;
        }
        public virtual void SetToggleGroup(ToggleGroup group) => toggle.group = group;

        public virtual void Select() => toggle.isOn = true;
    }
}
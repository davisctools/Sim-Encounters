using System;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    [RequireComponent(typeof(Toggle))]
    public class IconSelectorToggle : MonoBehaviour
    {
        public Image IconDisplay { get => iconDisplay; set => iconDisplay = value; }
        [SerializeField] private Image iconDisplay;

        protected Toggle Toggle => (toggle == null) ? toggle = GetComponent<Toggle>() : toggle;
        private Toggle toggle;

        public virtual Icon Icon { get; protected set; }

        public event Action<Icon> Selected;

        protected virtual void Start() => Toggle.onValueChanged.AddListener(OnToggleChanged);


        protected virtual void OnToggleChanged(bool value)
        {
            if (value) Selected?.Invoke(Icon);
        }

        public virtual void SetIcon(Icon icon, Sprite sprite)
        {
            Icon = icon;
            IconDisplay.sprite = sprite;
        }
        public virtual void SetToggleGroup(ToggleGroup group) => Toggle.group = group;

        public virtual void Select() => Toggle.isOn = true;
    }
}
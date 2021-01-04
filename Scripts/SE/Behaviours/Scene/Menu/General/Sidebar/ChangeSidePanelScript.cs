using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;

namespace ClinicalTools.SimEncounters
{
    [RequireComponent(typeof(Toggle))]
    public class ChangeSidePanelScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public event Action Selected;

        protected Toggle Toggle => (toggle == null) ? toggle = GetComponent<Toggle>() : toggle;
        private Toggle toggle;

        public TextMeshProUGUI Label { get => label; set => label = value; }
        [SerializeField] private TextMeshProUGUI label;
        public Color OnColor { get => onColor; set => onColor = value; }
        [SerializeField] private Color onColor;
        public Color OffColor { get => offColor; set => offColor = value; }
        [SerializeField] private Color offColor;
        public Color HoverColor { get => hoverColor; set => hoverColor = value; }
        [SerializeField] private Color hoverColor;


        protected virtual void Start()
        {
            Toggle.onValueChanged.AddListener(ToggleThis);
            ToggleThis(Toggle.isOn);
        }

        protected void ToggleThis(bool isOn)
        {
            Color textColor;
            // 195 dark and 115 light
            if (isOn) {
                Selected?.Invoke();
                textColor = OnColor;
            } else {
                textColor = OffColor;
            }
            Label.color = textColor;
            Toggle.interactable = !isOn;
        }

        public void OnPointerEnter(PointerEventData data)
        {
            if (!Toggle.isOn)
                Label.color = HoverColor;
        }

        public void OnPointerExit(PointerEventData data)
        {
            if (!Toggle.isOn)
                Label.color = OffColor;
        }

        public void Select() => Toggle.isOn = true;

        public void Show(string text)
        {
            Label.text = text;
            gameObject.SetActive(true);
            Select();
        }

        public void Display() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);

        public virtual bool IsOn() => Toggle.isOn;
    }
}
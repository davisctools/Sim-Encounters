using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.UI
{
    public abstract class BaseDropdownOption : MonoBehaviour
    {
        public virtual string Value { get; protected set; }
        public abstract event Action<string> Selected;
        public abstract void Display(string value);
        public abstract void SetActive(bool active);
        public abstract void Highlight();
        public abstract void RemoveHighlight();
        public class Factory : PlaceholderFactory<BaseDropdownOption> { }
    }

    public class DropdownOption : BaseDropdownOption
    {
        public TextMeshProUGUI OptionText { get => optionText; set => optionText = value; }
        [SerializeField] private TextMeshProUGUI optionText;

        public Button SelectButton { get => selectButton; set => selectButton = value; }
        [SerializeField] private Button selectButton;


        public override event Action<string> Selected;

        protected virtual void Awake() => SelectButton.onClick.AddListener(SelectButtonPressed);
        public override void Display(string value)
        {
            Value = value;
            OptionText.text = value;
        }
        public override void SetActive(bool active) => gameObject.SetActive(active);
        public override void Highlight() => SelectButton.image.color = new Color(.8f, 1f, 1f);
        public override void RemoveHighlight() => SelectButton.image.color = new Color(1f, 1f, 1f);
        protected virtual void SelectButtonPressed() => Selected?.Invoke(Value);
    }
}
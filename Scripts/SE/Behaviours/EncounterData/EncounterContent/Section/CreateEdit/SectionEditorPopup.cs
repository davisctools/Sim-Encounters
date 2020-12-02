
using ClinicalTools.UI;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class SectionEditorPopup : MonoBehaviour
    {
        public Button CancelButton { get => cancelButton; set => cancelButton = value; }
        [SerializeField] private Button cancelButton;
        public Button ApplyButton { get => applyButton; set => applyButton = value; }
        [SerializeField] private Button applyButton;
        public Button RemoveButton { get => removeButton; set => removeButton = value; }
        [SerializeField] private Button removeButton;

        public TMP_InputField NameField { get => nameField; set => nameField = value; }
        [SerializeField] private TMP_InputField nameField;

        public ColorUI Color { get => color; set => color = value; }
        [SerializeField] private ColorUI color;

        public IconSelectorUI IconSelector { get => iconSelector; set => iconSelector = value; }
        [SerializeField] private IconSelectorUI iconSelector;

        protected BaseMessageHandler MessageHandler { get; set; }
        protected BaseConfirmationPopup ConfirmationPopup { get; set; }
        [Inject] public virtual void Inject(BaseConfirmationPopup confirmationPopup, BaseMessageHandler messageHandler)
        {
            ConfirmationPopup = confirmationPopup; 
            MessageHandler = messageHandler;
        }

        protected virtual void Awake()
        {
            CancelButton.onClick.AddListener(Close);
            ApplyButton.onClick.AddListener(Apply);
            RemoveButton.onClick.AddListener(ConfirmRemove);
        }

        protected WaitableTask<Section> CurrentWaitableSection { get; set; }
        protected Section CurrentSection { get; set; }
        public virtual WaitableTask<Section> EditSection(Encounter encounter, Section section)
        {
            CurrentSection = section;

            if (CurrentWaitableSection?.IsCompleted() == false)
                CurrentWaitableSection.SetError(new Exception("New popup opened"));

            CurrentWaitableSection = new WaitableTask<Section>();
            gameObject.SetActive(true);

            NameField.text = section.Name;
            Color.Display(section.Color);
            IconSelector.Display(encounter, section.IconKey);

            return CurrentWaitableSection;
        }

        protected virtual void Apply()
        {
            if (string.IsNullOrEmpty(NameField.text)) {
                MessageHandler.ShowMessage("Name cannot be empty.", MessageType.Error);
                return;
            }

            CurrentSection.Name = NameField.text;
            CurrentSection.Color = Color.GetValue();
            CurrentSection.IconKey = IconSelector.Value;

            CurrentWaitableSection.SetResult(CurrentSection);
            CurrentWaitableSection = null;

            Close();
        }

        protected virtual void ConfirmRemove() => ConfirmationPopup.ShowConfirmation(Remove, "Confirm", "Are you sure you want to remove this section?");
        protected virtual void Remove()
        {
            CurrentWaitableSection.SetResult(null);
            Close();
        }

        protected virtual void Close()
        {
            if (CurrentWaitableSection?.IsCompleted() == false)
                CurrentWaitableSection.SetError(new Exception("Canceled"));

            gameObject.SetActive(false);
        }
    }
}
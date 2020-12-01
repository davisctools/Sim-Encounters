using ClinicalTools.UI;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class TabEditorPopup : MonoBehaviour
    {
        public Button CancelButton { get => cancelButton; set => cancelButton = value; }
        [SerializeField] private Button cancelButton;
        public Button ApplyButton { get => applyButton; set => applyButton = value; }
        [SerializeField] private Button applyButton;
        public Button RemoveButton { get => removeButton; set => removeButton = value; }
        [SerializeField] private Button removeButton;

        public TMP_InputField NameField { get => nameField; set => nameField = value; }
        [SerializeField] private TMP_InputField nameField;

        protected BaseMessageHandler MessageHandler { get; set; }
        protected BaseConfirmationPopup ConfirmationPopup { get; set; }
        [Inject]
        public virtual void Inject(BaseConfirmationPopup confirmationPopup, BaseMessageHandler messageHandler)
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
        protected WaitableTask<Tab> CurrentWaitableTab { get; set; }
        protected Tab CurrentTab { get; set; }
        public virtual WaitableTask<Tab> EditTab(Tab tab)
        {
            CurrentTab = tab;

            gameObject.SetActive(true);

            NameField.text = tab.Name;

            if (CurrentWaitableTab?.IsCompleted() == false)
                CurrentWaitableTab.SetError(new Exception("New popup opened"));
            CurrentWaitableTab = new WaitableTask<Tab>();
            return CurrentWaitableTab;
        }
        protected virtual void Apply()
        {
            if (string.IsNullOrEmpty(NameField.text)) {
                MessageHandler.ShowMessage("Name cannot be empty.", MessageType.Error);
                return;
            }
            CurrentTab.Name = NameField.text;

            CurrentWaitableTab.SetResult(CurrentTab);
            CurrentWaitableTab = null;

            Close();
        }

        protected virtual void ConfirmRemove() => ConfirmationPopup.ShowConfirmation(Remove, "Confirm", "Are you sure you want to remove this tab?");
        protected virtual void Remove()
        {
            CurrentWaitableTab.SetResult(null);
            Close();
        }

        protected virtual void Close()
        {
            if (CurrentWaitableTab?.IsCompleted() == false)
                CurrentWaitableTab.SetError(new Exception("Canceled"));

            gameObject.SetActive(false);
        }
    }
}
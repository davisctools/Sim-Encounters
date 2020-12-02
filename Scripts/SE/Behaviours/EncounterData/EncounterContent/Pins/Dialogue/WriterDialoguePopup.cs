using ClinicalTools.UI;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class WriterDialoguePopup : MonoBehaviour
    {
        public Button CancelButton { get => cancelButton; set => cancelButton = value; }
        [SerializeField] private Button cancelButton;
        public Button RemoveButton { get => removeButton; set => removeButton = value; }
        [SerializeField] private Button removeButton;
        public Button ApplyButton { get => applyButton; set => applyButton = value; }
        [SerializeField] private Button applyButton;

        public BaseWriterPanelsDrawer ConversationDrawer { get => conversationDrawer; set => conversationDrawer = value; }
        [SerializeField] private BaseWriterPanelsDrawer conversationDrawer;

        protected BaseConfirmationPopup ConfirmationPopup { get; set; }
        [Inject] public virtual void Inject(BaseConfirmationPopup confirmationPopup) => ConfirmationPopup = confirmationPopup;

        protected virtual void Awake()
        {
            CancelButton.onClick.AddListener(Close);
            RemoveButton.onClick.AddListener(ConfirmRemove);
            ApplyButton.onClick.AddListener(Apply);
        }

        protected WaitableTask<DialoguePin> CurrentWaitableDialogue { get; set; }
        protected DialoguePin CurrentDialogue { get; set; }
        public virtual WaitableTask<DialoguePin> EditDialogue(DialoguePin dialoguePin)
        {
            CurrentDialogue = dialoguePin;

            if (CurrentWaitableDialogue?.IsCompleted() == false)
                CurrentWaitableDialogue.SetError(new Exception("New popup opened"));

            CurrentWaitableDialogue = new WaitableTask<DialoguePin>();

            gameObject.SetActive(true);

            ConversationDrawer.DrawChildPanels(dialoguePin.Conversation);

            return CurrentWaitableDialogue;
        }
        protected virtual void Apply()
        {
            CurrentDialogue.Conversation = ConversationDrawer.SerializeChildren();
            CurrentWaitableDialogue.SetResult(CurrentDialogue);

            Close();
        }
        protected virtual void ConfirmRemove() => ConfirmationPopup.ShowConfirmation(Remove, "Confirm", "Are you sure you want to remove this dialogue?");
        protected virtual void Remove()
        {
            CurrentWaitableDialogue.SetResult(null);
            Close();
        }

        protected virtual void Close()
        {
            if (CurrentWaitableDialogue?.IsCompleted() == false)
                CurrentWaitableDialogue.SetError(new Exception("Canceled"));

            gameObject.SetActive(false);
        }
    }
}
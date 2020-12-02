using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ReaderDialoguePinButton : BaseUserDialoguePinDrawer
    {
        public virtual Button Button { get => button; set => button = value; }
        [SerializeField] private Button button;
        public virtual GameObject Completed { get => completed; set => completed = value; }
        [SerializeField] private GameObject completed;

        protected BaseUserDialoguePinDrawer DialoguePopup { get; set; }
        [Inject] public virtual void Inject(BaseUserDialoguePinDrawer dialoguePopup) => DialoguePopup = dialoguePopup;

        protected UserDialoguePin UserDialoguePin { get; set; }
        public override void Display(UserDialoguePin dialoguePin)
        {
            if (UserDialoguePin != null)
                UserDialoguePin.StatusChanged -= OnStatusChanged;

            UserDialoguePin = dialoguePin;
            UserDialoguePin.StatusChanged += OnStatusChanged;
            OnStatusChanged();

            Button.onClick.RemoveAllListeners();
            Button.onClick.AddListener(() => DialoguePopup.Display(dialoguePin));
        }

        protected virtual void OnStatusChanged()
        {
            if (Completed != null)
                Completed.SetActive(UserDialoguePin.IsRead());
        }

        protected virtual void OnDestroy()
        {
            if (UserDialoguePin != null)
                UserDialoguePin.StatusChanged -= OnStatusChanged;
        }
    }
}
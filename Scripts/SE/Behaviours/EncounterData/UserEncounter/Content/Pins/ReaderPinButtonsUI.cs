using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ReaderPinButtonsUI : BaseUserPinGroupDrawer
    {
        public virtual Transform ButtonsParent { get => buttonsParent; set => buttonsParent = value; }
        [SerializeField] private Transform buttonsParent;

        protected BaseUserReadMorePinDrawer.Pool ReadMoreButtonPool { get; set; }
        protected BaseUserDialoguePinDrawer.Pool DialogueButtonPool { get; set; }
        protected BaseUserQuizPinDrawer.Pool QuizButtonPool { get; set; }
        [Inject]
        public virtual void Inject(
            BaseUserReadMorePinDrawer.Pool readMoreButtonPool,
            BaseUserDialoguePinDrawer.Pool dialogueButtonPool,
            BaseUserQuizPinDrawer.Pool quizButtonPool)
        {
            ReadMoreButtonPool = readMoreButtonPool;
            DialogueButtonPool = dialogueButtonPool;
            QuizButtonPool = quizButtonPool;
        }

        protected BaseUserDialoguePinDrawer DialogueButton { get; set; }
        protected BaseUserQuizPinDrawer QuizButton { get; set; }
        protected BaseUserReadMorePinDrawer ReadMoreButton { get; set; }
        public override void Display(UserPinGroup pinGroup)
        {
            if (pinGroup == null || (pinGroup.DialoguePin == null && pinGroup.QuizPin == null && pinGroup.ReadMorePin == null)) {
                gameObject.SetActive(false);
                return;
            }

            transform.localScale = Vector3.one;
            DisplayDialoguePin(pinGroup.DialoguePin);
            DisplayQuizPin(pinGroup.QuizPin);
            DisplayReadMorePin(pinGroup.ReadMorePin);
        }
        protected virtual void DisplayDialoguePin(UserDialoguePin dialoguePin)
        {
            if (dialoguePin == null) {
                DespawnDialogueButton();
                return;
            }

            SpawnDialogueButton();
            DialogueButton.Display(dialoguePin);
        }

        protected virtual void DespawnDialogueButton()
        {
            if (DialogueButton == null)
                return;

            DialogueButtonPool.Despawn(DialogueButton);
            DialogueButton = null;
        }

        protected virtual void SpawnDialogueButton()
        {
            if (DialogueButton != null)
                return;

            DialogueButton = DialogueButtonPool.Spawn();
            DialogueButton.transform.localScale = Vector3.one;
            DialogueButton.transform.SetParent(ButtonsParent);
            DialogueButton.transform.SetAsFirstSibling();
        }

        protected virtual void DisplayQuizPin(UserQuizPin quizPin)
        {
            if (quizPin == null) {
                DespawnQuizButton();
                return;
            }

            SpawnQuizButton();
            QuizButton.Display(quizPin);
        }

        protected virtual void DespawnQuizButton()
        {
            if (QuizButton == null)
                return;

            QuizButtonPool.Despawn(QuizButton);
            QuizButton = null;
        }

        protected virtual void SpawnQuizButton()
        {
            if (QuizButton != null)
                return;

            QuizButton = QuizButtonPool.Spawn();
            QuizButton.transform.localScale = Vector3.one;
            QuizButton.transform.SetParent(ButtonsParent);
            QuizButton.transform.SetAsLastSibling();
        }

        protected virtual void DisplayReadMorePin(UserReadMorePin pin)
        {
            if (pin == null) {
                DespawnReadMoreButton();
                return;
            }

            SpawnReadMoreButton();
            ReadMoreButton.Display(pin);
        }

        protected virtual void DespawnReadMoreButton()
        {
            if (ReadMoreButton == null)
                return;

            ReadMoreButtonPool.Despawn(ReadMoreButton);
            ReadMoreButton = null;
        }

        protected virtual void SpawnReadMoreButton()
        {
            if (ReadMoreButton != null)
                return;

            ReadMoreButton = ReadMoreButtonPool.Spawn();
            ReadMoreButton.transform.localScale = Vector3.one;
            ReadMoreButton.transform.SetParent(ButtonsParent);
            ReadMoreButton.transform.SetAsLastSibling();
        }

        protected virtual void OnDestroy()
        {
            DespawnDialogueButton();
            DespawnQuizButton();
            DespawnReadMoreButton();
        }
    }
}
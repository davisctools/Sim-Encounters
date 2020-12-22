using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class WriterPinsDrawer : BaseWriterPinsDrawer
    {
        public Button ReadMorePinButton { get => readMorePinButton; set => readMorePinButton = value; }
        [SerializeField] private Button readMorePinButton;
        public Button DialoguePinButton { get => dialoguePinButton; set => dialoguePinButton = value; }
        [SerializeField] private Button dialoguePinButton;
        public Button QuizPinButton { get => quizPinButton; set => quizPinButton = value; }
        [SerializeField] private Button quizPinButton;

        protected virtual WriterReadMorePopup ReadMorePopup { get; set; }
        protected virtual WriterDialoguePopup DialoguePopup { get; set; }
        protected virtual WriterQuizPopup QuizPopup { get; set; }
        [Inject]
        public void Inject(
            WriterReadMorePopup readMorePopup,
            WriterDialoguePopup dialoguePopup,
            WriterQuizPopup quizPopup)
        {
            ReadMorePopup = readMorePopup;
            DialoguePopup = dialoguePopup;
            QuizPopup = quizPopup;
        }

        protected virtual void Awake()
        {
            ReadMorePinButton.onClick.AddListener(EditReadMore);
            DialoguePinButton.onClick.AddListener(EditDialogue);
            QuizPinButton.onClick.AddListener(EditQuiz);
        }

        protected PinGroup CurrentPinData { get; set; }
        public override void Display(PinGroup pinData)
        {
            CurrentPinData = pinData;

            ReadMorePinButton.image.color = GetButtonColor(CurrentPinData?.ReadMore != null);
            QuizPinButton.image.color = GetButtonColor(CurrentPinData?.Quiz != null);
            DialoguePinButton.image.color = GetButtonColor(CurrentPinData?.Dialogue != null);
        }

        public override PinGroup Serialize() => CurrentPinData;

        protected virtual void EditReadMore()
        {
            ReadMorePin pin = CurrentPinData.ReadMore ?? new ReadMorePin();
            var newDialogue = ReadMorePopup.EditReadMore(pin);
            newDialogue.AddOnCompletedListener(SetReadMore);
        }

        protected virtual void SetReadMore(TaskResult<ReadMorePin> pinResult)
        {
            if (pinResult.IsError())
                return;

            CurrentPinData.ReadMore = pinResult.Value;
            DialoguePinButton.image.color = GetButtonColor(pinResult.Value != null);
        }
        protected virtual void EditDialogue()
        {
            DialoguePin pin = CurrentPinData.Dialogue ?? new DialoguePin();
            var newDialogue = DialoguePopup.EditDialogue(pin);
            newDialogue.AddOnCompletedListener(SetDialogue);
        }

        protected virtual void SetDialogue(TaskResult<DialoguePin> dialogue)
        {
            if (dialogue.IsError())
                return;

            CurrentPinData.Dialogue = dialogue.Value;
            DialoguePinButton.image.color = GetButtonColor(dialogue.Value != null);
        }

        protected virtual void EditQuiz()
        {
            QuizPin pin = CurrentPinData.Quiz ?? new QuizPin();
            var newQuiz = QuizPopup.EditQuiz(pin);
            newQuiz.AddOnCompletedListener(SetQuiz);
        }

        protected virtual void SetQuiz(TaskResult<QuizPin> quiz)
        {
            if (quiz.IsError())
                return;

            CurrentPinData.Quiz = quiz.Value;
            QuizPinButton.image.color = GetButtonColor(quiz.Value != null);
        }

        protected Color OffColor { get; } = new Color(0.8823529f, 0.8823529f, 0.8823529f);
        protected Color OnColor { get; } = new Color(0.6603774f, 0.7452832f, 1f);
        protected Color GetButtonColor(bool isOn) => isOn ? OnColor : OffColor;
    }
}
using ClinicalTools.UI;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class WriterQuizPopup : MonoBehaviour
    {
        public Button CancelButton { get => cancelButton; set => cancelButton = value; }
        [SerializeField] private Button cancelButton;
        public Button RemoveButton { get => removeButton; set => removeButton = value; }
        [SerializeField] private Button removeButton;
        public Button ApplyButton { get => applyButton; set => applyButton = value; }
        [SerializeField] private Button applyButton;

        public BaseWriterPanelsDrawer QuestionsDrawer { get => questionsDrawer; set => questionsDrawer = value; }
        [SerializeField] private BaseWriterPanelsDrawer questionsDrawer;

        protected BaseConfirmationPopup ConfirmationPopup { get; set; }
        [Inject] public virtual void Inject(BaseConfirmationPopup confirmationPopup) => ConfirmationPopup = confirmationPopup;

        protected virtual void Awake()
        {
            CancelButton.onClick.AddListener(Close);
            RemoveButton.onClick.AddListener(ConfirmRemove);
            ApplyButton.onClick.AddListener(Apply);
        }

        protected WaitableTask<QuizPin> CurrentWaitableQuiz { get; set; }
        protected QuizPin CurrentQuiz { get; set; }
        public virtual WaitableTask<QuizPin> EditQuiz(QuizPin quizPin)
        {
            CurrentQuiz = quizPin;

            if (CurrentWaitableQuiz?.IsCompleted() == false)
                CurrentWaitableQuiz.SetError(new Exception("New popup opened"));

            CurrentWaitableQuiz = new WaitableTask<QuizPin>();

            gameObject.SetActive(true);

            QuestionsDrawer.DrawChildPanels(quizPin.Questions);

            return CurrentWaitableQuiz;
        }

        protected virtual void Apply()
        {
            CurrentQuiz.Questions = QuestionsDrawer.SerializeChildren();
            CurrentWaitableQuiz.SetResult(CurrentQuiz);

            Close();
        }

        protected virtual void ConfirmRemove() => ConfirmationPopup.ShowConfirmation(Remove, "Confirm", "Are you sure you want to remove this quiz?");
        protected virtual void Remove()
        {
            CurrentWaitableQuiz.SetResult(null);

            Close();
        }

        protected virtual void Close()
        {
            if (CurrentWaitableQuiz?.IsCompleted ()== false)
                CurrentWaitableQuiz.SetError(new Exception("Canceled"));

            gameObject.SetActive(false);
        }
    }
}
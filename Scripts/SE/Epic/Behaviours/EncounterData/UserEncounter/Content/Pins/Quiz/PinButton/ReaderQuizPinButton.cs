using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ReaderQuizPinButton : BaseUserQuizPinDrawer
    {
        public virtual Button Button { get => button; set => button = value; }
        [SerializeField] private Button button;
        public virtual GameObject Completed { get => completed; set => completed = value; }
        [SerializeField] private GameObject completed;

        protected BaseUserQuizPinDrawer QuizPopup { get; set; }
        [Inject] public virtual void Inject(BaseUserQuizPinDrawer quizPopup) => QuizPopup = quizPopup;


        protected UserQuizPin UserQuizPin { get; set; }
        public override void Display(UserQuizPin quizPin)
        {
            if (UserQuizPin != null)
                UserQuizPin.StatusChanged -= OnStatusChanged;

            UserQuizPin = quizPin;
            UserQuizPin.StatusChanged += OnStatusChanged;
            OnStatusChanged();

            Button.onClick.RemoveAllListeners();
            Button.onClick.AddListener(() => QuizPopup.Display(quizPin));
        }

        protected virtual void OnStatusChanged()
        {
            if (Completed != null)
                Completed.SetActive(UserQuizPin.IsRead());
        }

        protected virtual void OnDestroy()
        {
            if (UserQuizPin != null)
                UserQuizPin.StatusChanged -= OnStatusChanged;
        }
    }
}
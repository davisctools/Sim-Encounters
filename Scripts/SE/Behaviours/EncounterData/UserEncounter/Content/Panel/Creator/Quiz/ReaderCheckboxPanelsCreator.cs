using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    public class ReaderCheckboxPanelsCreator : ReaderGeneralPanelsCreator<ReaderOptionPanelBehaviour>
    {
        public Button FeedbackButton { get => feedbackButton; set => feedbackButton = value; }
        [SerializeField] private Button feedbackButton;

        protected virtual void Awake() => FeedbackButton.onClick.AddListener(ShowFeedback);

        protected virtual void ShowFeedback() {
            foreach (var child in Children.Values)
                child.GetFeedback();
        }
    }
}
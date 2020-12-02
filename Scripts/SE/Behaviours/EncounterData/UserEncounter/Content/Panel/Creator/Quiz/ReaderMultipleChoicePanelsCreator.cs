using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    public class ReaderMultipleChoicePanelsCreator : ReaderGeneralPanelsCreator<ReaderExclusiveOptionPanelBehaviour>
    {
        public Button FeedbackButton { get => feedbackButton; set => feedbackButton = value; }
        [SerializeField] private Button feedbackButton;
        public ToggleGroup ToggleGroup { get => toggleGroup; set => toggleGroup = value; }
        [SerializeField] private ToggleGroup toggleGroup;
        public Transform FeedbackParent { get => feedbackParent; set => feedbackParent = value; }
        [SerializeField] private Transform feedbackParent;

        protected virtual void Awake() => FeedbackButton.onClick.AddListener(ShowFeedback);

        protected override ReaderExclusiveOptionPanelBehaviour DrawPanel(UserPanel panel, bool active)
        {
            var panelBehaviour = base.DrawPanel(panel, active);
            if (ToggleGroup != null)
                panelBehaviour.SetToggleGroup(ToggleGroup);
            if (FeedbackParent != null)
                panelBehaviour.SetFeedbackParent(FeedbackParent);
            return panelBehaviour;
        }

        protected virtual void ShowFeedback()
        {
            foreach (var child in Children.Values)
                child.GetFeedback();
        }
    }
}
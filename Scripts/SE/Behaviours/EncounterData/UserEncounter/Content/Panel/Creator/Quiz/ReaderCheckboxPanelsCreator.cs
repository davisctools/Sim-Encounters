using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    public class ReaderCheckboxPanelsCreator : ReaderGeneralPanelsCreator<ReaderOptionPanelBehaviour>
    {
        public Button FeedbackButton { get => feedbackButton; set => feedbackButton = value; }
        [SerializeField] private Button feedbackButton;

        protected virtual void Start() => FeedbackButton.onClick.AddListener(ShowFeedback);

        protected override ReaderOptionPanelBehaviour DrawPanel(UserPanel panel, bool active)
        {
            var panelBehaviour = base.DrawPanel(panel, active);
            if (panelBehaviour == null)
                return null;

            panelBehaviour.SelectChanged += OptionSelectChanged;

            return panelBehaviour;
        }

        protected virtual void OptionSelectChanged() => FeedbackButton.interactable = true;

        protected virtual void ShowFeedback()
        {
            FeedbackButton.interactable = false;
            foreach (var child in Children.Values)
                child.GetFeedback();
        }
    }
}
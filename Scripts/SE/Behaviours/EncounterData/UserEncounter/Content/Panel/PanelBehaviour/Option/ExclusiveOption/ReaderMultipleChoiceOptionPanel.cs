using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    public class ReaderMultipleChoiceOptionPanel : ReaderExclusiveOptionPanelBehaviour
    {
        protected override BaseChildUserPanelsDrawer ChildPanelsDrawer { get => childPanelsDrawer; }
        [SerializeField] private BaseChildUserPanelsDrawer childPanelsDrawer = null;
        protected override BaseUserPinGroupDrawer PinsDrawer { get => pinsDrawer; }
        [SerializeField] private BaseUserPinGroupDrawer pinsDrawer = null;
        protected override bool SetReadOnSelect => true;

        public virtual Toggle Toggle { get => toggle; set => toggle = value; }
        [SerializeField] private Toggle toggle;
        public virtual ReaderFeedbackUI Feedback { get => feedback; set => feedback = value; }
        [SerializeField] private ReaderFeedbackUI feedback;

        public override void GetFeedback()
        {
            if (Toggle.isOn)
                Feedback.ShowFeedback(true);
            else
                Feedback.CloseFeedback();
        }

        public override void SetToggleGroup(ToggleGroup toggleGroup) => Toggle.group = toggleGroup;
        public override void SetFeedbackParent(Transform feedbackParent) => Feedback.transform.SetParent(feedbackParent);
    }
}
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    public class ReaderDialogueOption : BaseReaderDialogueOption
    {
        protected override BaseChildUserPanelsDrawer ChildPanelsDrawer { get => childPanelsDrawer; }
        [SerializeField] private BaseChildUserPanelsDrawer childPanelsDrawer = null;
        protected override BaseUserPinGroupDrawer PinsDrawer => null;
        protected override bool SetReadOnSelect => true;

        public virtual Toggle Toggle { get => toggle; set => toggle = value; }
        [SerializeField] private Toggle toggle;
        public virtual Color OnColor { get => onColor; set => onColor = value; }
        [SerializeField] private Color onColor;
        protected Color OffColor { get; set; }
        public virtual Image Border { get => border; set => border = value; }
        [SerializeField] private Image border;
        public virtual ReaderFeedbackUI Feedback { get => feedback; set => feedback = value; }


        [SerializeField] private ReaderFeedbackUI feedback;

        public override event Action<BaseReaderDialogueOption> CorrectlySelected;

        protected virtual void Awake()
        {
            OffColor = OnColor; 
            Toggle.onValueChanged.AddListener(GetFeedback);
        }

        protected virtual void GetFeedback(bool isOn)
        {
            if (isOn) {
                Border.color = OnColor;
                Feedback.ShowFeedback(isOn);
                if (Feedback.OptionType == OptionType.Correct)
                    CorrectlySelected?.Invoke(this);
            } else {
                Border.color = OffColor;
                Feedback.CloseFeedback();
            }
        }

        public override void SetGroup(ToggleGroup group) => Toggle.group = group;

        public override void SetFeedbackParent(Transform parent) => Feedback.SetParent(parent);
        public override void CloseFeedback() => Feedback.CloseFeedback();
    }
}
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    public class ReaderCheckboxOptionPanel : ReaderOptionPanelBehaviour
    {
        protected override BaseChildUserPanelsDrawer ChildPanelsDrawer { get => childPanelsDrawer; }
        [SerializeField] private BaseChildUserPanelsDrawer childPanelsDrawer = null;
        protected override BaseUserPinGroupDrawer PinsDrawer { get => pinsDrawer; }
        [SerializeField] private BaseUserPinGroupDrawer pinsDrawer = null;

        public virtual Toggle Toggle { get => toggle; set => toggle = value; }
        [SerializeField] private Toggle toggle;
        public virtual ReaderFeedbackUI Feedback { get => feedback; set => feedback = value; }
        [SerializeField] private ReaderFeedbackUI feedback;

        public override event Action SelectChanged;

        protected override bool SetReadOnSelect => true;

        protected virtual void Start() => Toggle.onValueChanged.AddListener(OnToggleChanged);
        protected virtual void OnToggleChanged(bool value) => SelectChanged?.Invoke();

        public override void GetFeedback()
        {
            if (Toggle.isOn)
                Feedback.ShowFeedback(true);
            else if (Feedback.OptionType == OptionType.Correct)
                Feedback.ShowFeedback(false);
            else
                Feedback.CloseFeedback();
        }
    }
}
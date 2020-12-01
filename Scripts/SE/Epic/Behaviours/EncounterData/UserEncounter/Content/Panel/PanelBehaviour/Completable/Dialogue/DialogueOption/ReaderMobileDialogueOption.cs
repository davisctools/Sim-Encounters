﻿using System;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    public class ReaderMobileDialogueOption : BaseReaderDialogueOption
    {
        protected override BaseChildUserPanelsDrawer ChildPanelsDrawer { get => childPanelsDrawer; }
        [SerializeField] private BaseChildUserPanelsDrawer childPanelsDrawer = null;
        protected override BaseUserPinGroupDrawer PinsDrawer => null;

        public virtual GameObject CorrectlySelectedObject { get => correctlySelectedObject; set => correctlySelectedObject = value; }
        [SerializeField] private GameObject correctlySelectedObject;
        public virtual GameObject NormalObject { get => normalObject; set => normalObject = value; }
        [SerializeField] private GameObject normalObject;
        public virtual Toggle Toggle { get => toggle; set => toggle = value; }
        [SerializeField] private Toggle toggle;
        public virtual Sprite OnSprite { get => onSprite; set => onSprite = value; }
        [SerializeField] private Sprite onSprite;
        protected Sprite OffSprite { get; set; }
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
            OffSprite = Toggle.image.sprite;
            Toggle.onValueChanged.AddListener(GetFeedback);
        }

        protected virtual void GetFeedback(bool isOn)
        {
            if (!isOn) {
                Toggle.interactable = true;
                Border.color = OffColor;
                Toggle.image.sprite = OffSprite;
                Feedback.CloseFeedback();
                return;
            } 

            if (Feedback.OptionType != OptionType.Correct) {
                Toggle.interactable = false;
                Border.color = OnColor;
                Toggle.image.sprite = OnSprite;
                Feedback.ShowFeedback(isOn);
                return;
            }

            CorrectlySelectedObject.SetActive(true);
            NormalObject.SetActive(false);
            CorrectlySelected?.Invoke(this);
        }

        public override void SetGroup(ToggleGroup group) => Toggle.group = group;

        public override void SetFeedbackParent(Transform parent) => Feedback.SetParent(parent);
        public override void CloseFeedback() => Feedback.CloseFeedback();
    }
}
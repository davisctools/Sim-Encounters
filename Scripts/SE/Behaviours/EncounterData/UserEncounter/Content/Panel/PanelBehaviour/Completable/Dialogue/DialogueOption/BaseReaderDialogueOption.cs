using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{

    public class DialogueOptionCorrectlySelectedEventArgs : EventArgs
    {
        public BaseReaderDialogueOption DialogueOption { get; }
        public UserPanel Panel { get; }

        public DialogueOptionCorrectlySelectedEventArgs(BaseReaderDialogueOption dialogueOption, UserPanel panel)
        {
            DialogueOption = dialogueOption;
            Panel = panel;
        }
    }
    public delegate void DialogueOptionCorrectlySelectedHandler(object sender, DialogueOptionCorrectlySelectedEventArgs e);

    public abstract class BaseReaderDialogueOption : BaseReaderPanelBehaviour
    {
        public abstract event DialogueOptionCorrectlySelectedHandler CorrectlySelected;
        public abstract void SetGroup(ToggleGroup group);
        public abstract void SetFeedbackParent(Transform parent);
        public abstract void CloseFeedback();
    }
}
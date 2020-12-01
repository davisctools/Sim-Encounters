using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseReaderDialogueOption : BaseReaderPanelBehaviour
    {
        public abstract event Action<BaseReaderDialogueOption> CorrectlySelected;
        public abstract void SetGroup(ToggleGroup group);
        public abstract void SetFeedbackParent(Transform parent);
        public abstract void CloseFeedback();
    }
}
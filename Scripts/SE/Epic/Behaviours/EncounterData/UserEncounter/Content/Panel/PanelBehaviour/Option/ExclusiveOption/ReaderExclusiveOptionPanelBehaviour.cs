using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    public abstract class ReaderExclusiveOptionPanelBehaviour : ReaderOptionPanelBehaviour
    {
        public abstract void SetToggleGroup(ToggleGroup toggleGroup);
        public abstract void SetFeedbackParent(Transform feedbackParent);
    }
}
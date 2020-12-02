using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public interface IReaderFeedbackUI
    {
        OptionType OptionType { get; }

        void CloseFeedback();
        void SetParent(Transform parent);
        void ShowFeedback(bool isOn);
    }
}
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public interface IFeedbackColorManager
    {
        Color GetColor(OptionType optionType);
        Color GetDefaultColor();
    }
}
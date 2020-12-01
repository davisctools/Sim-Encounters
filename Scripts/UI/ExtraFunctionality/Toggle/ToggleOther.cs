using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.UI
{
    /// <summary>
    /// Toggles another toggle when this toggle is toggled
    /// </summary>
    public class ToggleOther : MonoBehaviour
    {
        public Toggle other;

        public void Toggle() => other.isOn = !other.isOn;
    }
}
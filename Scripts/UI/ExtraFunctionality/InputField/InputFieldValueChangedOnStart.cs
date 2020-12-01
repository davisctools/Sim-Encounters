using TMPro;
using UnityEngine;

namespace ClinicalTools.UI
{
    /// <summary>
    /// Calls the input field's on value changed method at start.
    /// </summary>
    [RequireComponent(typeof(TMP_InputField))]
    public class InputFieldValueChangedOnStart : MonoBehaviour
    {
        protected virtual void Start()
        {
            var input = gameObject.GetComponent<TMP_InputField>();
            NextFrame.Function(
                delegate { input.onValueChanged.Invoke(input.text); });
        }
    }
}
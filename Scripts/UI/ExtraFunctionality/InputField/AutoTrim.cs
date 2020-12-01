using TMPro;
using UnityEngine;

namespace ClinicalTools.UI
{
    // Automatically trims the text in the field after editing, so all devices behave consistently.
    [RequireComponent(typeof(TMP_InputField))]
    public class AutoTrim : MonoBehaviour
    {
        TMP_InputField field;

        // Use this for initialization
        void Start()
        {
            Debug.LogWarning("autotrim");
            field = GetComponent<TMP_InputField>();
            field.onEndEdit.AddListener(
                delegate { field.text = field.text.Trim(); }
            );
        }
    }
}
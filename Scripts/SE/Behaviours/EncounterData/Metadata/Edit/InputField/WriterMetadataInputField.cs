using TMPro;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    [RequireComponent(typeof(TMP_InputField))]
    public abstract class WriterMetadataInputField : WriterMetadataBehaviour
    {
        protected TMP_InputField InputField 
            => (inputField == null) ? inputField = GetComponent<TMP_InputField>() : inputField;
        private TMP_InputField inputField;
    }
}
using TMPro;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    [RequireComponent(typeof(TMP_InputField))]
    public class EncounterCompletionCodeInputField : EncounterMetadataBehaviour
    {
        protected TMP_InputField InputField
            => (inputField == null) ? inputField = GetComponent<TMP_InputField>() : inputField;
        private TMP_InputField inputField;

        protected override void OnMetadataSelected(object sender, EncounterMetadataSelectedEventArgs eventArgs)
        {
            if (eventArgs.Metadata is IWebCompletion webCompletion)
                InputField.text = webCompletion.CompletionCode;
        }
    }
}
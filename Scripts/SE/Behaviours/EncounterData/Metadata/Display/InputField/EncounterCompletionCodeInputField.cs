using TMPro;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    [RequireComponent(typeof(TMP_InputField))]
    public class EncounterCompletionCodeInputField : EncounterMetadataBehaviour
    {
        private TMP_InputField inputField;
        protected TMP_InputField InputField {
            get {
                if (inputField == null)
                    inputField = GetComponent<TMP_InputField>();
                return inputField;
            }
        }

        protected override void OnMetadataSelected(object sender, EncounterMetadataSelectedEventArgs eventArgs)
        {
            if (eventArgs.Metadata is IWebCompletion webCompletion)
                InputField.text = webCompletion.CompletionCode;
        }
    }
}
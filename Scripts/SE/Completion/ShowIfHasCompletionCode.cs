using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class ShowIfHasCompletionCode : EncounterMetadataBehaviour
    {
        [SerializeField] private bool hideOnCode;
        protected override void OnMetadataSelected(object sender, EncounterMetadataSelectedEventArgs eventArgs)
            => gameObject.SetActive(
                (eventArgs.Metadata is IWebCompletion webCompletion 
                    && !string.IsNullOrWhiteSpace(webCompletion.CompletionCode))
                ^ hideOnCode);
    }
}
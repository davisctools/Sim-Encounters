using TMPro;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class EncounterTitleLabel : EncounterMetadataLabel
    {
        [SerializeField] private bool capitalizeAll;

        protected override void OnMetadataSelected(object sender, EncounterMetadataSelectedEventArgs eventArgs)
            => Label.text = capitalizeAll ? eventArgs.Metadata.Title.ToUpper() : eventArgs.Metadata.Title;
    }
}
using TMPro;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class EncounterSubtitleLabel : EncounterMetadataLabel
    {
        protected override void OnMetadataSelected(object sender, EncounterMetadataSelectedEventArgs eventArgs)
            => Label.text = eventArgs.Metadata.Subtitle;
    }
}
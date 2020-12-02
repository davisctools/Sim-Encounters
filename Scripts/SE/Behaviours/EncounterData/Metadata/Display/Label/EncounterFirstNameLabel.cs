using TMPro;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class EncounterFirstNameLabel : EncounterMetadataLabel
    {
        protected override void OnMetadataSelected(object sender, EncounterMetadataSelectedEventArgs eventArgs)
        {
            if (eventArgs.Metadata is INamed named) 
                Label.text = named.Name.FirstName;
        }
    }
}
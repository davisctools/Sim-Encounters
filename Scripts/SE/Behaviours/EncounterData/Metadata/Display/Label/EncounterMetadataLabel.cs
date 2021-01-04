using TMPro;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public abstract class EncounterMetadataLabel : EncounterMetadataBehaviour
    {
        protected TextMeshProUGUI Label => (label == null) ? label = GetComponent<TextMeshProUGUI>() : label;
        private TextMeshProUGUI label;
    }
}
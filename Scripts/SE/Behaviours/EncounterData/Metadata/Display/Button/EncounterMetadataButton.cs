using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    [RequireComponent(typeof(Button))]
    public abstract class EncounterMetadataButton : EncounterMetadataBehaviour
    {
        protected Button Button => (button == null) ? button = GetComponent<Button>() : button;
        private Button button;
    }
}
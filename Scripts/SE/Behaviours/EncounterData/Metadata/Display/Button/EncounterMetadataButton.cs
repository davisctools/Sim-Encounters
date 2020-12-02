using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    [RequireComponent(typeof(Button))]
    public abstract class EncounterMetadataButton : EncounterMetadataBehaviour
    {
        private Button button;
        protected Button Button {
            get {
                if (button == null)
                    button = GetComponent<Button>();
                return button;
            }
        }
    }
}
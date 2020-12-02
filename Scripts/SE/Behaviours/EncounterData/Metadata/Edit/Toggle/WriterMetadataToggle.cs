using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    [RequireComponent(typeof(Toggle))]
    public abstract class WriterMetadataToggle : WriterMetadataBehaviour
    {
        private Toggle toggle;
        protected Toggle Toggle {
            get {
                if (toggle == null)
                    toggle = GetComponent<Toggle>();
                return toggle;
            }
        }
    }
}
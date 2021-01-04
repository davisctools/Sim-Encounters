using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    [RequireComponent(typeof(Toggle))]
    public abstract class WriterMetadataToggle : WriterMetadataBehaviour
    {
        protected Toggle Toggle => (toggle == null) ? toggle = GetComponent<Toggle>() : toggle;
        private Toggle toggle;
    }
}
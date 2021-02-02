using UnityEngine.UI.Extensions;

namespace ClinicalTools.UI
{
    public class FixedAccordionElement : AccordionElement
    {
#if UNITY_EDITOR
        /// <summary>
        /// The OnValidate method normally throws a ton of errors in the editor if the Accordion
        /// Group is on an object that starts inactive, so this prevents that from happening.
        /// This also prevents the error from happening when editing an accordion element prefab.
        /// </summary>
        protected override void OnValidate()
        {
            if (transform.parent != null && transform.parent.parent != null && gameObject.activeInHierarchy)
                base.OnValidate();
        }
#endif
    }
}
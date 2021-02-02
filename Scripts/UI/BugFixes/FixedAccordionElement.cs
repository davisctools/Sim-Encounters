using UnityEngine.UI.Extensions;

namespace ClinicalTools.UI
{
    public class FixedAccordionElement : AccordionElement
    {
#if UNITY_EDITOR
        /// <summary>
        /// The OnValidate method normally throws a ton of errors in the editor 
        /// if the Accordiong Group is on an object that starts inactive, 
        /// so this prevents that from hapepening.
        /// </summary>
        protected override void OnValidate()
        {
            if (transform.parent != null && gameObject.activeInHierarchy)
                base.OnValidate();
        }
#endif
    }
}
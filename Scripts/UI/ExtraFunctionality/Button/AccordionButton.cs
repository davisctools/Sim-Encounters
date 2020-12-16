using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

namespace ClinicalTools.UI
{
    [RequireComponent(typeof(Button))]
    public class AccordionButton : MonoBehaviour
    {
        protected AccordionElement AccordionElement { get; set; }

        protected virtual void Awake()
        {
            AccordionElement = GetComponentInParent<AccordionElement>();
            var button = GetComponent<Button>();
            if (button != null)
                button.onClick.AddListener(ToggleAccordionElementOpen);
        }

        protected virtual void ToggleAccordionElementOpen() => AccordionElement.isOn = !AccordionElement.isOn;
    }
}
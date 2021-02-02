using UnityEngine;
using UnityEngine.EventSystems;
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
            GetComponent<Button>().onClick.AddListener(ToggleAccordionElementOpen);
        }

        protected virtual void ToggleAccordionElementOpen()
        {
            AccordionElement.isOn = !AccordionElement.isOn;
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}
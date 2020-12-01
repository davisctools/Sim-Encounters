using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ClinicalTools.UI
{
    public class DisabledToggleTooltip : MonoBehaviour, IPointerDownHandler
    {
        public Toggle Toggle { get => toggle; set => toggle = value; }
        [SerializeField] private Toggle toggle;
        public BaseTooltip Tooltip { get => tooltip; set => tooltip = value; }
        [SerializeField] private BaseTooltip tooltip;

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            if (!Toggle.interactable)
                Tooltip.Show();
        }
    }
}
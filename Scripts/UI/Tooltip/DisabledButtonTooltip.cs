using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ClinicalTools.UI
{
    public class DisabledButtonTooltip : MonoBehaviour, IPointerDownHandler
    {
        public Button Button { get => button; set => button = value; }
        [SerializeField] private Button button;
        public BaseTooltip Tooltip { get => tooltip; set => tooltip = value; }
        [SerializeField] private BaseTooltip tooltip;

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            if (!Button.interactable)
                Tooltip.Show();
        }
    }
}
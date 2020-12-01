using UnityEngine;
using UnityEngine.EventSystems;

namespace ClinicalTools.UI
{
    public class TabScrollButtonScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public bool IsDown { get; set; } = false;

        public virtual void OnPointerDown(PointerEventData eventData) => IsDown = true;
        public virtual void OnPointerUp(PointerEventData eventData) => IsDown = false;

        public virtual void SetActive(bool value)
        {
            if (!value)
                IsDown = false;
            gameObject.SetActive(value);
        }
    }
}
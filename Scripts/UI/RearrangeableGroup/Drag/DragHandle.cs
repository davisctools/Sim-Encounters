using System;
using UnityEngine.EventSystems;

namespace ClinicalTools.UI
{
    public class DragHandle : BaseDragHandle, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public override event Action StartDragging;

        public void OnPointerDown(PointerEventData eventData) => StartDragging?.Invoke();
        public void OnPointerEnter(PointerEventData eventData) => MouseInput.Instance.SetCursorState(CursorState.Draggable);
        public void OnPointerExit(PointerEventData eventData) => MouseInput.Instance.RemoveCursorState(CursorState.Draggable);
    }
}
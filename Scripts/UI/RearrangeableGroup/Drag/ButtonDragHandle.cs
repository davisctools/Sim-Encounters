using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ClinicalTools.UI
{
    public class ButtonDragHandle : BaseDragHandle, IPointerDownHandler
    {
        public override event Action StartDragging;

        private const float DRAG_TOLERANCE = 50;
        protected virtual bool StartingDrag { get; set; } = false;
        protected virtual float ClickPosition { get; set; }

        public void OnPointerDown(PointerEventData eventData) {
            StartingDrag = true;
            ClickPosition = eventData.position.x;
        }

        protected virtual void Update()
        {
            if (!StartingDrag)
                return;
            if (!Input.GetMouseButton(0)) {
                StartingDrag = false;
                return;
            }

            var distance = Mathf.Abs(Input.mousePosition.x - ClickPosition);
            if (distance > DRAG_TOLERANCE) {
                StartingDrag = false;
                StartDragging?.Invoke();
            }
        }
    }
}
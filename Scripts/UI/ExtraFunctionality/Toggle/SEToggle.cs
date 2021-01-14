using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ClinicalTools.UI
{
    public class SEToggle : Toggle
    {
#if UNITY_WEBGL
        protected bool IsMouseOver { get; set; }
        protected bool MouseCursorSet { get; set; }
        public override void OnPointerEnter(PointerEventData eventData)
        {
            IsMouseOver = true;
            CheckToSetMouse();
            base.OnPointerEnter(eventData);
        }


        public override void OnPointerExit(PointerEventData eventData)
        {
            IsMouseOver = false;
            CheckToUnsetMouse();
            base.OnPointerExit(eventData);
        }

        protected override void OnDisable()
        {
            IsMouseOver = false;
            CheckToUnsetMouse();
            base.OnDisable();
        }

        protected virtual void Update()
        {
            CheckToSetMouse();
            CheckToUnsetMouse();
        }

        protected virtual void CheckToSetMouse()
        {
            if (!IsMouseOver || MouseCursorSet || !interactable || MouseInput.Instance == null)
                return;

            MouseCursorSet = true;
            MouseInput.Instance.SetCursorState(CursorState.Clickable);
        }

        protected virtual void CheckToUnsetMouse()
        {
            if (!MouseCursorSet || MouseInput.Instance == null || (IsMouseOver && interactable))
                return;

            MouseCursorSet = false;
            MouseInput.Instance.RemoveCursorState(CursorState.Clickable);
        }
#endif
    }
}
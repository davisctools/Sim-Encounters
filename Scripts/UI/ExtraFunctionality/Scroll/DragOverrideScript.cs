using UnityEngine.EventSystems;

namespace ClinicalTools.UI
{
    public class DragOverrideScript : CEScrollRect
    {
        public static bool DragAllowed { get; set; } = true;

        protected override void Start()
        {
#if MOBILE
        inertia = true;
        decelerationRate = .05f;
#endif

            base.Start();
        }

        protected virtual bool CanDrag()
        {
#if MOBILE
        return DragAllowed && (MouseInput.Instance == null || MouseInput.Instance.CanDrag) 
            && content.rect.height > viewport.rect.height
            && Input.touches.Length < 2;
#else
            return false;
#endif
        }

        public override void OnBeginDrag(PointerEventData data)
        {
            if (CanDrag())
                base.OnBeginDrag(data);
        }

        public override void OnDrag(PointerEventData data)
        {
            if (CanDrag())
                base.OnDrag(data);
        }

        public override void OnEndDrag(PointerEventData data)
        {
            if (CanDrag())
                base.OnEndDrag(data);
        }
    }
}
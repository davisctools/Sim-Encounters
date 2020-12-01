using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ClinicalTools.UI
{
    public class TabField : TabGroup
    {
        public RectTransform TopRect { get => topRect; set => topRect = value; }
        [SerializeField, Tooltip("Topmost transform to always be in frame when this is tabbed to.")]
        private RectTransform topRect;
        public RectTransform BottomRect { get => bottomRect; set => bottomRect = value; }
        [SerializeField, Tooltip("Bottommost transform to always be in frame when this is tabbed to.")]
        private RectTransform bottomRect;

        private Selectable selectable;
        private DragOverrideScript scrollbar;

        protected bool WasSelected { get; set; }

        protected override void Start()
        {
            base.Start();

            selectable = GetComponent<Selectable>();
            selectable.navigation = new Navigation();

            if (TopRect == null)
                TopRect = GetComponent<RectTransform>();
            if (BottomRect == null)
                BottomRect = GetComponent<RectTransform>();

            scrollbar = GetComponentInParent<DragOverrideScript>();
        }


        protected virtual void SelectLast()
        {
            var rowFields = AllFields();

            var index = rowFields.IndexOf(this);
            while (--index >= 0) {
                var nextField = rowFields[index];
                if (!nextField.Visible)
                    continue;

                nextField.Select();
                break;
            }
        }

        protected virtual void SelectNext()
        {
            var rowFields = AllFields();

            var index = rowFields.IndexOf(this);
            while (++index < rowFields.Count) {
                var nextField = rowFields[index];
                if (!nextField.Visible)
                    continue;

                nextField.Select();
                break;
            }
        }

        protected virtual void Select()
        {
            selectable.Select();
            if (scrollbar != null && scrollbar.viewport.rect.height < scrollbar.content.rect.height)
                scrollbar.SnapTo(TopRect, BottomRect);
        }

        protected virtual void Update()
        {
            if (EventSystem.current.currentSelectedGameObject != gameObject) {
                WasSelected = false;
                return;
            }
            if (!WasSelected) {
                WasSelected = true;
                return;
            }

            if (!Input.GetKeyDown(KeyCode.Tab))
                return;

            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                SelectLast();
            else
                SelectNext();
        }
    }
}
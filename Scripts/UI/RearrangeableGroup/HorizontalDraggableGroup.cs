using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.UI
{
    public class HorizontalDraggableGroup : DraggableGroup
    {
        public bool ControlPlaceholderWidth { get => controlPlaceholderWidth; set => controlPlaceholderWidth = value; }
        [SerializeField] private bool controlPlaceholderWidth;

        protected LayoutElement PlaceholderLayoutElement { get; set; }
        protected virtual void Awake()
        {
            if (ControlPlaceholderWidth)
                PlaceholderLayoutElement = Placeholder.GetComponent<LayoutElement>();
        }

        protected override void DragStarted(IDraggable draggable, Vector3 mousePosition)
        {
            base.DragStarted(draggable, mousePosition);

            if (ControlPlaceholderWidth) {
                if (PlaceholderLayoutElement)
                    PlaceholderLayoutElement.minWidth = draggable.RectTransform.sizeDelta.x;
                else
                    Debug.LogError("Cannot control placeholder width if placeholder does not have a Layout Element component.");
            }
        }

        protected override void SetPosition(IDraggable draggable, Vector3 mousePosition)
        {
            var position = draggable.RectTransform.position;

            var worldCorners = new Vector3[4];
            ChildrenParent.GetWorldCorners(worldCorners);
            position.x = Mathf.Clamp(mousePosition.x, worldCorners[0].x, worldCorners[2].x) - Offset;

            draggable.RectTransform.position = position;
        }

        protected override bool BeforeNeighbor(IDraggable neighbor, Vector3 mousePosition)
            => DistanceFromMouse(neighbor.RectTransform, mousePosition) - Offset < 0;
        protected override bool AfterNeighbor(IDraggable neighbor, Vector3 mousePosition)
            => DistanceFromMouse(neighbor.RectTransform, mousePosition) - Offset > 0;
        protected override float DistanceFromMouse(RectTransform rectTransform, Vector3 mousePosition)
        {
            var worldCorners = new Vector3[4];
            rectTransform.GetWorldCorners(worldCorners); 
            return mousePosition.x - worldCorners[0].x;
        }
    }
}
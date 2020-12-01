using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.UI
{
    public class VerticalDraggableGroup : DraggableGroup
    {
        public bool ControlPlaceholderHeight { get => controlPlaceholderHeight; set => controlPlaceholderHeight = value; }
        [SerializeField] private bool controlPlaceholderHeight;
        
        protected LayoutElement PlaceholderLayoutElement { get; set; }
        protected virtual void Awake()
        {
            if (ControlPlaceholderHeight)
                PlaceholderLayoutElement = Placeholder.GetComponent<LayoutElement>();
        }

        protected override void DragStarted(IDraggable draggable, Vector3 mousePosition)
        {
            base.DragStarted(draggable, mousePosition);

            if (ControlPlaceholderHeight) {
                if (PlaceholderLayoutElement)
                    PlaceholderLayoutElement.minHeight = draggable.RectTransform.sizeDelta.y;
                else
                    Debug.LogError("Cannot control placeholder height if placeholder does not have a Layout Element component.");
            }
        }

        protected override void SetPosition(IDraggable draggable, Vector3 mousePosition)
        {
            var position = draggable.RectTransform.position;

            var worldCorners = new Vector3[4];
            ChildrenParent.GetWorldCorners(worldCorners);
            position.y = Mathf.Clamp(mousePosition.y, worldCorners[0].y, worldCorners[1].y);

            draggable.RectTransform.position = position;
        }

        protected override bool BeforeNeighbor(IDraggable neighbor, Vector3 mousePosition)
            => DistanceFromMouse(neighbor.RectTransform, mousePosition) - Offset > 0;
        protected override bool AfterNeighbor(IDraggable neighbor, Vector3 mousePosition)
            => DistanceFromMouse(neighbor.RectTransform, mousePosition) - Offset < 0;
        protected override float DistanceFromMouse(RectTransform rectTransform, Vector3 mousePosition)
        {
            var worldCorners = new Vector3[4];
            rectTransform.GetWorldCorners(worldCorners);
            return mousePosition.y - worldCorners[0].y;
        }
    }
}
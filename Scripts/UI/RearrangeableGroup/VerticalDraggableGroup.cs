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


        // Offset seems to be making it a bit harder to move certain things to the desired position,
        // so disabling it would help with somethings.
        // I'll need to keep an out to make sure this isn't breaking other things.
        protected override float GetOffset(RectTransform rectTransform, Vector3 mousePosition) => 0;
        
        protected override float DistanceFromMouse(RectTransform rectTransform, Vector3 mousePosition)
        {
            var worldCorners = new Vector3[4];
            rectTransform.GetWorldCorners(worldCorners);

            // Average should make moving feel more natural, but I need to keep an eye out for parts of the code that may have broken
             return mousePosition.y - ((worldCorners[0].y + worldCorners[1].y) / 2);
            //return mousePosition.y - worldCorners[0].y;
        }
    }
}
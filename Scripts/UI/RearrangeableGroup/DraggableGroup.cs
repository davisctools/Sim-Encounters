using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.UI
{
    public abstract class DraggableGroup : BaseRearrangeableGroup
    {
        public GameObject Placeholder { get => placeholder; set => placeholder = value; }
        [SerializeField] private GameObject placeholder;
        public virtual RectTransform ChildrenParent => (RectTransform)transform;

        public override event RearrangedEventHandler Rearranged;
        protected List<IDraggable> DraggableObjects { get; } = new List<IDraggable>();

        public override void Add(IDraggable draggable)
        {
            draggable.RectTransform.SetParent(ChildrenParent);

            DraggableObjects.Add(draggable);
            draggable.DragStarted += DragStarted;
            draggable.DragEnded += DragEnded;
            draggable.Dragging += Dragging;

            // would be optimal if this was only done at the end of adding a number of items
            // moving it to a start method and doing it here if not started would minimize the issue
            // checking for it at the beginning of update could also work, but I generally prefer to minimize work in Update
            Placeholder.transform.SetAsLastSibling();
        }

        public override void Remove(IDraggable draggable)
        {
            DraggableObjects.Remove(draggable);
            draggable.DragStarted -= DragStarted;
            draggable.DragEnded -= DragEnded;
            draggable.Dragging -= Dragging;
        }
        public override void Clear() => DraggableObjects.Clear();


        protected virtual float Offset { get; set; }
        protected virtual int InitialIndex { get; set; }
        protected virtual int Index { get; set; }

        protected virtual void DragStarted(IDraggable draggable, Vector3 mousePosition)
        {
            Index = DraggableObjects.IndexOf(draggable);
            InitialIndex = Index;
            DraggableObjects.RemoveAt(Index);

            draggable.LayoutElement.ignoreLayout = true;
            draggable.LayoutElement.layoutPriority = 10000;

            Placeholder.SetActive(true);
            SetPlaceholderIndex(draggable);
            draggable.RectTransform.SetAsLastSibling();

            Offset = DistanceFromMouse(draggable.RectTransform, mousePosition);
        }

        protected virtual void SetPlaceholderIndex(IDraggable draggable)
            => Placeholder.transform.SetSiblingIndex(draggable.RectTransform.GetSiblingIndex());

        protected virtual void Dragging(IDraggable draggable, Vector3 mousePosition)
        {
            SetPosition(draggable, mousePosition);

            if (Index > 0) {
                var upperNeighbor = DraggableObjects[Index - 1];
                if (BeforeNeighbor(upperNeighbor, mousePosition)) {
                    SetPlaceholderIndex(upperNeighbor);
                    Index--;
                    return;
                }
            }
            if (Index < DraggableObjects.Count) {
                var lowerNeighbor = DraggableObjects[Index];
                if (AfterNeighbor(lowerNeighbor, mousePosition)) {
                    SetPlaceholderIndex(lowerNeighbor);
                    Index++;
                }
            }
        }
        protected virtual void DragEnded(IDraggable draggable, Vector3 mousePosition)
        {
            draggable.RectTransform.SetSiblingIndex(Placeholder.transform.GetSiblingIndex());
            Placeholder.SetActive(false);
            draggable.LayoutElement.ignoreLayout = false;
            draggable.LayoutElement.layoutPriority = 1;

            DraggableObjects.Insert(Index, draggable);
            if (InitialIndex != Index) {
                var args = new RearrangedEventArgs2(InitialIndex, Index, draggable, DraggableObjects);
                Rearranged?.Invoke(this, args);
            }

            Placeholder.transform.SetAsLastSibling();
        }

        protected abstract void SetPosition(IDraggable draggable, Vector3 mousePosition);
        protected abstract bool BeforeNeighbor(IDraggable neighbor, Vector3 mousePosition);
        protected abstract bool AfterNeighbor(IDraggable neighbor, Vector3 mousePosition);
        protected abstract float DistanceFromMouse(RectTransform rectTransform, Vector3 mousePosition);
    }
}
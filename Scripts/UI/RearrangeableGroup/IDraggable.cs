using System;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.UI
{
    public interface IDraggable
    {
        RectTransform RectTransform { get; }
        LayoutElement LayoutElement { get; }

        event Action<IDraggable, Vector3> DragStarted;
        event Action<IDraggable, Vector3> DragEnded;
        event Action<IDraggable, Vector3> Dragging;

        void StartDrag(Vector3 mousePosition);
        void EndDrag(Vector3 mousePosition);
        void Drag(Vector3 mousePosition);
    }
}
using ClinicalTools.UI;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    public abstract class ReaderOrderablePanelBehaviour : BaseReaderPanelBehaviour, IDraggable
    {
        public abstract RectTransform RectTransform { get; }
        public abstract LayoutElement LayoutElement { get; }

        public abstract event Action<IDraggable, Vector3> DragStarted;
        public abstract event Action<IDraggable, Vector3> DragEnded;
        public abstract event Action<IDraggable, Vector3> Dragging;

        public abstract void Drag(Vector3 mousePosition);
        public abstract void EndDrag(Vector3 mousePosition);
        public abstract void StartDrag(Vector3 mousePosition);

        public abstract void SetColor(Color color);
    }
}
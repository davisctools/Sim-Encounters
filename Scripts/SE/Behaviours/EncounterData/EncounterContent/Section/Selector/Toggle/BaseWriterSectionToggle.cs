using ClinicalTools.UI;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseWriterSectionToggle : MonoBehaviour, IDraggable
    {
        public virtual RectTransform RectTransform => (RectTransform)transform;

        public abstract LayoutElement LayoutElement { get; }


        public abstract event Action<Section> Selected;
        public abstract event Action<Section> Edited;
        public abstract event Action<Section> Deleted;
        public event Action<IDraggable, Vector3> DragStarted;
        public event Action<IDraggable, Vector3> DragEnded;
        public event Action<IDraggable, Vector3> Dragging;

        public abstract void Display(ContentEncounter encounter, Section section);
        public abstract void SetToggleGroup(ToggleGroup group);
        public abstract void Select();

        public virtual void StartDrag(Vector3 mousePosition) => DragStarted?.Invoke(this, mousePosition);
        public virtual void EndDrag(Vector3 mousePosition) => DragEnded?.Invoke(this, mousePosition);
        public virtual void Drag(Vector3 mousePosition) => Dragging?.Invoke(this, mousePosition);

        public class Pool : SceneMonoMemoryPool<BaseWriterSectionToggle>
        {
            public Pool(SignalBus signalBus) : base(signalBus) { }
        }
    }
}
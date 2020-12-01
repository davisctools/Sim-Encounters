﻿using System;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.UI
{
    public abstract class BaseDraggable : MonoBehaviour, IDraggable
    {
        public string DisplayName { get => displayName; set => displayName = value; }
        [SerializeField] private string displayName;

        public RectTransform RectTransform => (RectTransform)transform;
        public abstract LayoutElement LayoutElement { get; }


        public abstract event Action<IDraggable, Vector3> DragStarted;
        public abstract event Action<IDraggable, Vector3> DragEnded;
        public abstract event Action<IDraggable, Vector3> Dragging;

        public abstract void Drag(Vector3 mousePosition);
        public abstract void EndDrag(Vector3 mousePosition);
        public abstract void StartDrag(Vector3 mousePosition);
    }
}
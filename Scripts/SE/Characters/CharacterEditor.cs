using ClinicalTools.UI;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    [RequireComponent(typeof(RectTransform))]
    public class CharacterEditor : MonoBehaviour, IDraggable
    {
        [SerializeField] private BaseColorEditor colorEditor;
        [SerializeField] private BaseIconSelector iconSelector;
        [SerializeField] private TMP_InputField nameField;
        [SerializeField] private Button deleteButton;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private LayoutElement layoutElement;

        public Character Character { get; protected set; }

        public RectTransform RectTransform => (RectTransform)transform;

        public LayoutElement LayoutElement => layoutElement;

        public event Action<IDraggable, Vector3> DragStarted;
        public event Action<IDraggable, Vector3> DragEnded;
        public event Action<IDraggable, Vector3> Dragging;

        public virtual void Display(Character character)
        {
            Character = character;
            nameField.text = character.Name;
            colorEditor.Display(character.Color);
            iconSelector.Display(character.Icon);
        }

        public virtual void Serialize()
        {
            Character.Name = nameField.text;
            Character.Color = colorEditor.GetValue();
            Character.Icon = iconSelector.GetValue();
        }

        public virtual void StartDrag(Vector3 mousePosition)
        {
            canvasGroup.alpha = .5f;
            DragStarted?.Invoke(this, mousePosition);
        }
        public virtual void Drag(Vector3 mousePosition) => Dragging?.Invoke(this, mousePosition);
        public virtual void EndDrag(Vector3 mousePosition)
        {
            canvasGroup.alpha = 1;
            DragEnded?.Invoke(this, mousePosition);
        }

        public class Factory : PlaceholderFactory<CharacterEditor> { }
    }
}
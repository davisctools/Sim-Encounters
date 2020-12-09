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
        [SerializeField] private BaseIconOptionSelector iconSelector;
        [SerializeField] private TMP_InputField roleField;
        [SerializeField] private TMP_InputField nameField;
        [SerializeField] private Button deleteButton;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private LayoutElement layoutElement;
        public BaseDragHandle DragHandle { get => dragHandle; set => dragHandle = value; }
        [SerializeField] private BaseDragHandle dragHandle;

        public Character Character { get; protected set; }

        public RectTransform RectTransform => (RectTransform)transform;

        public LayoutElement LayoutElement => layoutElement;

        public event Action<IDraggable, Vector3> DragStarted;
        public event Action<IDraggable, Vector3> DragEnded;
        public event Action<IDraggable, Vector3> Dragging;

        protected BaseConfirmationPopup ConfirmationPopup { get; set; }
        [Inject] public virtual void Inject(BaseConfirmationPopup confirmationPopup) => ConfirmationPopup = confirmationPopup;

        protected virtual void Start()
        {
            deleteButton.onClick.AddListener(ConfirmDelete);
            DragHandle.StartDragging += StartDragging;
        }

        protected virtual void StartDragging() => MouseInput.Instance.RegisterDraggable(this);
        protected virtual void ConfirmDelete() => ConfirmationPopup.ShowConfirmation(Delete, "Confirm", "Are you sure you want to remove this entry?");
        protected virtual void Delete() => Destroy(gameObject);
        public virtual void Display(Character character)
        {
            Character = character;
            nameField.text = character.Name;
            roleField.text = character.Role;
            colorEditor.Display(character.Color);
            iconSelector.Display(character.Icon);
        }

        public virtual void Serialize()
        {
            Character.Name = nameField.text;
            Character.Role = roleField.text;
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
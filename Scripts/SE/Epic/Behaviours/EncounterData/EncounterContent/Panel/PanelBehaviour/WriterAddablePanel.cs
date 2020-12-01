using ClinicalTools.UI;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class WriterAddablePanel : BaseWriterAddablePanel
    {
        public override event Action<IDraggable, Vector3> DragStarted;
        public override event Action<IDraggable, Vector3> DragEnded;
        public override event Action<IDraggable, Vector3> Dragging;
        public override event Action Deleted;

        public override LayoutElement LayoutElement => layoutElement;
        [SerializeField] private LayoutElement layoutElement = null;
        public BaseDragHandle DragHandle { get => dragHandle; set => dragHandle = value; }
        [SerializeField] private BaseDragHandle dragHandle;
        public CanvasGroup CanvasGroup { get => canvasGroup; set => canvasGroup = value; }
        [SerializeField] private CanvasGroup canvasGroup;
        public Button DeleteButton { get => deleteButton; set => deleteButton = value; }
        [SerializeField] private Button deleteButton;

        protected override BaseWriterPanelsDrawer ChildPanelCreator { get => childPanelCreator; }
        [SerializeField] private BaseWriterPanelsDrawer childPanelCreator;

        protected override BaseWriterPinsDrawer PinsDrawer { get => pinsDrawer; }
        [SerializeField] private BaseWriterPinsDrawer pinsDrawer;


        protected BaseConfirmationPopup ConfirmationPopup { get; set; }
        [Inject] public virtual void Inject(BaseConfirmationPopup confirmationPopup) => ConfirmationPopup = confirmationPopup;

        protected virtual void Awake()
        {
            DeleteButton.onClick.AddListener(ConfirmDelete);
            DragHandle.StartDragging += StartDragging;
        }
        protected virtual void StartDragging() => MouseInput.Instance.RegisterDraggable(this);
        protected virtual void ConfirmDelete() => ConfirmationPopup.ShowConfirmation(Delete, "Confirm", "Are you sure you want to remove this entry?");
        protected virtual void Delete()
        {
            Deleted?.Invoke();
            Destroy(gameObject);
        }

        public override void StartDrag(Vector3 mousePosition)
        {
            CanvasGroup.alpha = .5f;
            DragStarted?.Invoke(this, mousePosition);
        }
        public override void Drag(Vector3 mousePosition) => Dragging?.Invoke(this, mousePosition);
        public override void EndDrag(Vector3 mousePosition)
        {
            CanvasGroup.alpha = 1;
            DragEnded?.Invoke(this, mousePosition);
        }
    }
}
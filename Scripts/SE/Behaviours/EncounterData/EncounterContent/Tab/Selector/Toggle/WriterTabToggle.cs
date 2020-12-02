using ClinicalTools.UI;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class WriterTabToggle : BaseWriterTabToggle
    {
        public SelectableToggle SelectToggle { get => selectToggle; set => selectToggle = value; }
        [SerializeField] private SelectableToggle selectToggle;
        public TextMeshProUGUI NameLabel { get => nameLabel; set => nameLabel = value; }
        [SerializeField] private TextMeshProUGUI nameLabel;
        public Button EditButton { get => editButton; set => editButton = value; }
        [SerializeField] private Button editButton;
        public override LayoutElement LayoutElement { get => layoutElement; }
        [SerializeField] private LayoutElement layoutElement = null;
        public BaseDragHandle DragHandle { get => dragHandle; set => dragHandle = value; }
        [SerializeField] private BaseDragHandle dragHandle;

        protected virtual TabEditorPopup TabEditorPopup { get; set; }
        [Inject] public void Inject(TabEditorPopup tabEditorPopup) => TabEditorPopup = tabEditorPopup;

        public override event Action Selected;
        public override event Action<Tab> Edited;
        public override event Action<Tab> Deleted;
        protected virtual void Awake()
        {
            SelectToggle.Selected += OnSelected;
            SelectToggle.Unselected += OnUnselected;
            EditButton.onClick.AddListener(Edit);

            DragHandle.StartDragging += StartDragging;
        }
        protected virtual void StartDragging()
        {
            MouseInput.Instance.RegisterDraggable(this);
        }

        protected Tab CurrentTab { get; set; }
        public override void Display(Tab tab)
        {
            CurrentTab = tab;
            NameLabel.text = tab.Name;
        }

        public override void SetToggleGroup(ToggleGroup group) => SelectToggle.SetToggleGroup(group);

        public override void Select() => SelectToggle.Select();

        protected virtual void OnSelected()
        {
            Selected?.Invoke();
            EditButton.gameObject.SetActive(true);
        }
        protected virtual void OnUnselected()
        {
            EditButton.gameObject.SetActive(false);
        }

        protected virtual void Edit()
        {
            var tab = TabEditorPopup.EditTab(CurrentTab);
            tab.AddOnCompletedListener(FinishEdit);
        }

        protected virtual void FinishEdit(TaskResult<Tab> editedTab)
        {
            if (editedTab.IsError())
                return;

            if (editedTab.Value == null) {
                Deleted?.Invoke(CurrentTab);
                return;
            }

            CurrentTab = editedTab.Value;
            Edited?.Invoke(CurrentTab);

            Display(CurrentTab);
        }
    }
}
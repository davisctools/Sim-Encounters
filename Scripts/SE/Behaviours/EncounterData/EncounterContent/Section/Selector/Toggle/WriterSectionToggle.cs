using ClinicalTools.UI;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class WriterSectionToggle : BaseWriterSectionToggle
    {
        public SelectableToggle SelectToggle { get => selectToggle; set => selectToggle = value; }
        [SerializeField] private SelectableToggle selectToggle;
        public Image Image { get => image; set => image = value; }
        [SerializeField] private Image image;
        public Image Icon { get => icon; set => icon = value; }
        [SerializeField] private Image icon;
        public TextMeshProUGUI NameLabel { get => nameLabel; set => nameLabel = value; }
        [SerializeField] private TextMeshProUGUI nameLabel;
        public Button EditButton { get => editButton; set => editButton = value; }
        [SerializeField] private Button editButton;
        public override LayoutElement LayoutElement { get => layoutElement; }
        [SerializeField] private LayoutElement layoutElement = null;
        public BaseDragHandle DragHandle { get => dragHandle; set => dragHandle = value; }
        [SerializeField] private BaseDragHandle dragHandle;

        protected virtual SectionEditorPopup SectionEditorPopup { get; set; }
        [Inject] public void Inject(SectionEditorPopup sectionEditorPopup) => SectionEditorPopup = sectionEditorPopup;

        public override event Action<Section> Selected;
        public override event Action<Section> Deleted;
        public override event Action<Section> Edited;

        protected virtual void Awake()
        {         
            SelectToggle.Selected += OnSelected;
            SelectToggle.Unselected += OnUnselected;
            EditButton.onClick.AddListener(Edit);

            DragHandle.StartDragging += () => MouseInput.Instance.RegisterDraggable(this);
        }

        protected Encounter CurrentEncounter { get; set; }
        protected Section CurrentSection { get; set; }
        public override void Display(Encounter encounter, Section section)
        {
            CurrentEncounter = encounter;
            CurrentSection = section;

            Image.color = section.Color;
            var icons = encounter.Content.Icons;
            if (section.IconKey != null && icons.ContainsKey(section.IconKey))
                Icon.sprite = icons[section.IconKey];

            NameLabel.text = section.Name;
        }

        public override void Select() => SelectToggle.Select();

        public override void SetToggleGroup(ToggleGroup group) => SelectToggle.SetToggleGroup(group);

        protected virtual void OnSelected()
        {
            Selected?.Invoke(CurrentSection);
            EditButton.gameObject.SetActive(true);
        }
        protected virtual void OnUnselected()
        {
            EditButton.gameObject.SetActive(false);
        }

        protected virtual void Edit()
        {
            var section =  SectionEditorPopup.EditSection(CurrentEncounter, CurrentSection);
            section.AddOnCompletedListener(FinishEdit);
        }

        protected virtual void FinishEdit(TaskResult<Section> editedSection)
        {
            if (editedSection.IsError())
                return;
            
            if (editedSection.Value == null) {
                Deleted?.Invoke(CurrentSection);
                return;
            }

            CurrentSection = editedSection.Value;
            Edited?.Invoke(CurrentSection);

            Display(CurrentEncounter, CurrentSection);
        }
    }
}
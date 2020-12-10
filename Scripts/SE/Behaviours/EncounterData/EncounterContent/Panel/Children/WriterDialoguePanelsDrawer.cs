using ClinicalTools.Collections;
using ClinicalTools.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class WriterDialoguePanelsDrawer : BaseWriterPanelsDrawer
    {
        public Button PatientEntryButton { get => patientEntryButton; set => patientEntryButton = value; }
        [SerializeField] private Button patientEntryButton;
        public Button ProviderEntryButton { get => providerEntryButton; set => providerEntryButton = value; }
        [SerializeField] private Button providerEntryButton;
        public Button InstructorEntryButton { get => instructorEntryButton; set => instructorEntryButton = value; }
        [SerializeField] private Button instructorEntryButton;

        public Button ChoiceButton { get => choiceButton; set => choiceButton = value; }
        [SerializeField] private Button choiceButton;
        public Button TextboxButton { get => textboxButton; set => textboxButton = value; }
        [SerializeField] private Button textboxButton;

        public Button CopyButton { get => copyButton; set => copyButton = value; }
        [SerializeField] private Button copyButton;
        public Button PasteButton { get => pasteButton; set => pasteButton = value; }
        [SerializeField] private Button pasteButton;

        public BaseRearrangeableGroup ReorderableGroup { get => reorderableGroup; set => reorderableGroup = value; }
        [SerializeField] private BaseRearrangeableGroup reorderableGroup;
        public BaseWriterAddablePanel TextboxPrefab { get => textboxPrefab; set => textboxPrefab = value; }
        [SerializeField] private BaseWriterAddablePanel textboxPrefab;
        public BaseWriterAddablePanel EntryPrefab { get => entryPrefab; set => entryPrefab = value; }
        [SerializeField] private BaseWriterAddablePanel entryPrefab;
        public BaseWriterAddablePanel ChoicePrefab { get => choicePrefab; set => choicePrefab = value; }
        [SerializeField] private BaseWriterAddablePanel choicePrefab;

        protected virtual OrderedCollection<BaseWriterPanel> WriterPanels { get; set; } = new OrderedCollection<BaseWriterPanel>();
        protected virtual List<BaseWriterAddablePanel> PanelPrefabs { get; } = new List<BaseWriterAddablePanel>();


        protected BaseWriterPanel.Factory PanelFactory { get; set; }
        protected BaseConfirmationPopup ConfirmationPopup { get; set; }
        [Inject]
        public virtual void Inject(BaseWriterPanel.Factory panelFactory, BaseConfirmationPopup confirmationPopup)
        {
            PanelFactory = panelFactory;
            ConfirmationPopup = confirmationPopup;
        }

        protected virtual void Awake()
        {
            PanelPrefabs.Add(EntryPrefab);
            PanelPrefabs.Add(ChoicePrefab);
            PanelPrefabs.Add(TextboxPrefab);

            CopyButton.onClick.AddListener(CopyPanels);
            PasteButton.onClick.AddListener(ConfirmPastePanels);

            PatientEntryButton.onClick.AddListener(AddPatientEntry);
            ProviderEntryButton.onClick.AddListener(AddProviderEntry);
            InstructorEntryButton.onClick.AddListener(AddInstructorEntry);
            ChoiceButton.onClick.AddListener(AddChoice);
            TextboxButton.onClick.AddListener(AddTextbox);
        }

        protected static OrderedCollection<Panel> CopiedPanels { get; set; }
        protected virtual void CopyPanels() => CopiedPanels = SerializeChildren();
        protected virtual void ConfirmPastePanels()
        {
            if (CopiedPanels == null || CopiedPanels.Count == 0)
                return;

            ConfirmationPopup.ShowConfirmation(PastePanels, "Paste Panels",
                $"Confirm pasting in the {CopiedPanels.Count} copied panels.");
        }
        protected virtual void PastePanels() => AddChildPanels(CopiedPanels);

        private void AddPatientEntry() => CreateEntryPanel("Patient", new Color(0.106f, 0.722f, 0.059f));
        private void AddProviderEntry() => CreateEntryPanel("Provider", new Color(0, 0.2509804f, 0.9568627f));
        private void AddInstructorEntry() => CreateEntryPanel("Instructor", new Color(0.569f, 0.569f, 0.569f));
        private void CreateEntryPanel(string characterName, Color characterColor)
        {
            var panel = new Panel("Entry");
            panel.Values.Add("characterName", characterName);
            panel.Values.Add("charColor", characterColor.ToString());

            var panelUI = InstantiatePanel(EntryPrefab);
            ReorderableGroup.Add(panelUI);
            panelUI.Select(this, new PanelSelectedEventArgs(panel));
            WriterPanels.Add(panelUI);
        }

        protected virtual void AddTextbox()
        {
            var panelUI = InstantiatePanel(TextboxPrefab);
            ReorderableGroup.Add(panelUI);
            panelUI.Select(this, new PanelSelectedEventArgs(null));
            WriterPanels.Add(panelUI);
        }
        protected virtual void AddChoice()
        {
            var panelUI = InstantiatePanel(ChoicePrefab);
            ReorderableGroup.Add(panelUI);
            panelUI.Select(this, new PanelSelectedEventArgs(null));
            WriterPanels.Add(panelUI);
        }

        public override void DrawChildPanels(OrderedCollection<Panel> childPanels)
        {
            foreach (var writerPanel in WriterPanels.Values) {
                if (writerPanel != null)
                    Destroy(writerPanel.gameObject);
            }

            WriterPanels = new OrderedCollection<BaseWriterPanel>();
            AddChildPanels(childPanels);
        }

        protected virtual void AddChildPanels(OrderedCollection<Panel> childPanels)
        {
            var childrenPanelManager = new ChildrenPanelManager();
            foreach (var panel in childPanels) {
                var prefab = childrenPanelManager.ChoosePrefab(PanelPrefabs, panel.Value);
                var panelUI = InstantiatePanel(prefab);
                ReorderableGroup.Add(panelUI);
                panelUI.Select(this, new PanelSelectedEventArgs(panel.Value));
                WriterPanels.Add(panel.Key, panelUI);
            }
        }


        public override void DrawDefaultChildPanels()
            => WriterPanels = new OrderedCollection<BaseWriterPanel>();

        public override OrderedCollection<Panel> SerializeChildren()
        {
            var childrenPanelManager = new ChildrenPanelManager();
            return childrenPanelManager.SerializeChildren(WriterPanels);
        }

        protected virtual BaseWriterAddablePanel InstantiatePanel(BaseWriterAddablePanel writerPanelPrefab)
        {
            var writerPanel = (BaseWriterAddablePanel)PanelFactory.Create(writerPanelPrefab);
            writerPanel.transform.SetParent(ReorderableGroup.transform);
            writerPanel.transform.localScale = Vector3.one;
            return writerPanel;
        }
    }
}
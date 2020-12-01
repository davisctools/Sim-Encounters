using ClinicalTools.UI;
using ClinicalTools.UI.Extensions;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class TabCreatorPopup : MonoBehaviour
    {
        public List<Button> CancelButtons { get => cancelButtons; set => cancelButtons = value; }
        [SerializeField] private List<Button> cancelButtons;
        public Button CreateButton { get => createButton; set => createButton = value; }
        [SerializeField] private Button createButton;

        public TMP_InputField NameField { get => nameField; set => nameField = value; }
        [SerializeField] private TMP_InputField nameField;

        public ToggleGroup TabGroups { get => tabGroups; set => tabGroups = value; }
        [SerializeField] private ToggleGroup tabGroups;
        public ToggleGroup TabTypes { get => tabTypes; set => tabTypes = value; }
        [SerializeField] private ToggleGroup tabTypes;

        public TextMeshProUGUI DescriptionLabel { get => descriptionLabel; set => descriptionLabel = value; }
        [SerializeField] private TextMeshProUGUI descriptionLabel;

        protected WaitableTask<Tab> CurrentWaitableTab { get; set; }

        protected BaseMessageHandler MessageHandler { get; set; }
        protected TabTypeButtonUI.Factory TabTypeButtonFactory { get; set; }
        [Inject]
        public virtual void Inject(BaseMessageHandler messageHandler, TabTypeButtonUI.Factory tabTypeButtonFactory)
        {
            MessageHandler = messageHandler;
            TabTypeButtonFactory = tabTypeButtonFactory;
        }
        public virtual WaitableTask<Tab> CreateTab()
        {
            CurrentWaitableTab?.SetError(new Exception("New popup opened"));
            CurrentWaitableTab = new WaitableTask<Tab>();

            gameObject.SetActive(true);
            TabGroups.allowSwitchOff = true;
            TabTypes.allowSwitchOff = true;
            NameField.text = "";
            foreach (Transform child in TabTypes.transform)
                Destroy(child.gameObject);
            if (SelectedGroupButton != null)
                SelectedGroupButton.Deselect();
            DescriptionLabel.text = "";

            return CurrentWaitableTab;
        }

        protected virtual void Awake()
        {
            foreach (var cancelButton in CancelButtons)
                cancelButton.onClick.AddListener(() => Close());

            CreateButton.onClick.AddListener(() => AddTab());

            var tabTypesInfo = new TabTypesInfo();
            AddCategories(tabTypesInfo.Groups);
        }

        protected string SelectedPrefab { get; set; }
        protected virtual void AddTab()
        {
            if (string.IsNullOrWhiteSpace(SelectedPrefab)) {
                MessageHandler.ShowMessage("Must select a tab template.", MessageType.Error);
                return;
            }
            var name = NameField.text?.Trim();
            if (string.IsNullOrEmpty(name))
                name = DefaultName;

            var tab = new Tab(SelectedPrefab, name);
            CurrentWaitableTab.SetResult(tab);
            CurrentWaitableTab = null;

            Close();
        }

        protected virtual void Close()
        {
            if (CurrentWaitableTab != null) {
                CurrentWaitableTab.SetError(new Exception("Canceled"));
                CurrentWaitableTab = null;
            }
            gameObject.SetActive(false);
        }

        public void AddCategories(Dictionary<string, List<TabType>> tabTypeGroups)
        {
            foreach (var group in tabTypeGroups) {
                var groupButton = TabTypeButtonFactory.Create();
                groupButton.transform.SetParent(TabGroups.transform);
                groupButton.transform.localScale = Vector3.one;

                groupButton.Label.text = group.Key;
                groupButton.Toggle.group = TabGroups;
                groupButton.Toggle.AddOnSelectListener(() => GroupSelected(groupButton, group.Value));
            }
        }

        protected virtual TabTypeButtonUI SelectedGroupButton { get; set; }
        protected virtual void GroupSelected(TabTypeButtonUI groupButton, List<TabType> tabTypes)
        {
            SelectedGroupButton = groupButton;
            TabGroups.allowSwitchOff = false;
            foreach (Transform child in TabTypes.transform)
                Destroy(child.gameObject);

            TabTypes.allowSwitchOff = true;
            DescriptionLabel.text = "";

            foreach (var tabType in tabTypes) {
                var tabButton = TabTypeButtonFactory.Create();
                tabButton.transform.SetParent(TabTypes.transform);
                tabButton.transform.localScale = Vector3.one;

                tabButton.Label.text = tabType.Display;
                tabButton.Toggle.group = TabTypes;
                tabButton.Toggle.AddOnSelectListener(() => TypeSelected(tabType.Display, tabType));
            }
        }

        protected string DefaultName { get; set; }
        protected virtual void TypeSelected(string defaultName, TabType tabType)
        {
            DefaultName = defaultName;
            TabTypes.allowSwitchOff = false;
            DescriptionLabel.text = tabType.Description;
            SelectedPrefab = tabType.Prefab;
        }
    }
}
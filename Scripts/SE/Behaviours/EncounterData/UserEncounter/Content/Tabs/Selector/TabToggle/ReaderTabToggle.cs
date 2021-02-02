using ClinicalTools.UI;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    public class ReaderTabToggle : BaseSelectableTabToggle
    {
        public SelectableToggle SelectToggle { get => selectToggle; set => selectToggle = value; }
        [SerializeField] private SelectableToggle selectToggle;
        public TextMeshProUGUI NameLabel { get => nameLabel; set => nameLabel = value; }
        [SerializeField] private TextMeshProUGUI nameLabel;
        public GameObject Visited { get => visited; set => visited = value; }
        [SerializeField] private GameObject visited;

        protected virtual void Awake() => SelectToggle.Selected += Selected;
        protected virtual void Selected()
        {
            TabSelector.Select(this, new UserTabSelectedEventArgs(CurrentTab, ChangeType.JumpTo));
        }

        protected UserTab CurrentTab { get; set; }
        public override void Initialize(UserTab tab)
        {
            CurrentTab = tab;
            NameLabel.text = tab.Data.Name;

            StatusChanged();
            tab.StatusChanged += StatusChanged;
            base.Initialize(tab);
        }

        public override void SetToggleGroup(ToggleGroup group) 
            => SelectToggle.SetToggleGroup(group);

        protected virtual void StatusChanged() => Visited.SetActive(CurrentTab.IsRead());
        public override void Select() => SelectToggle.Select();
        protected virtual void OnDestroy() => CurrentTab.StatusChanged -= StatusChanged;
    }
}
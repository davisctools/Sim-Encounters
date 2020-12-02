using ClinicalTools.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ReaderDialogueOptionsDrawer : BaseChildUserPanelsDrawer
    {
        protected List<BaseReaderDialogueOption> PanelOptions { get => panelOptions; set => panelOptions = value; }
        [SerializeField] private List<BaseReaderDialogueOption> panelOptions = new List<BaseReaderDialogueOption>();
        protected ToggleGroup ToggleGroup { get => toggleGroup; set => toggleGroup = value; }
        [SerializeField] private ToggleGroup toggleGroup;
        public virtual Transform OptionsParent { get => optionsParent; set => optionsParent = value; }
        [SerializeField] private Transform optionsParent;
        public virtual Transform CompletedOptionsParent { get => completedOptionsParent; set => completedOptionsParent = value; }
        [SerializeField] private Transform completedOptionsParent;

        protected BaseReaderPanelBehaviour.Factory ReaderPanelFactory { get; set; }
        protected IPanelCompletedHandler PanelCompletedHandler { get; set; }
        [Inject]
        public virtual void Inject(
            BaseReaderPanelBehaviour.Factory readerPanelFactory,
            IPanelCompletedHandler panelCompletedHandler)
        {
            ReaderPanelFactory = readerPanelFactory;
            PanelCompletedHandler = panelCompletedHandler;
        }

        public override void Display(OrderedCollection<UserPanel> panels, bool active)
        {
            foreach (Transform child in transform)
                Destroy(child.gameObject);

            foreach (var panel in panels.Values)
                DisplayOption(panel, active);
        }

        protected virtual void DisplayOption(UserPanel panel, bool active)
        {
            var prefab = GetChildPanelPrefab(panel);
            if (prefab == null)
                return;

            var option = (BaseReaderDialogueOption)ReaderPanelFactory.Create(prefab);
            option.transform.SetParent(OptionsParent);
            option.transform.localScale = Vector3.one;
            option.Select(this, new UserPanelSelectedEventArgs(panel, true));
            option.SetGroup(toggleGroup);
            option.CorrectlySelected += OnOptionSelectedCorrectly;
        }

        private void OnOptionSelectedCorrectly(BaseReaderDialogueOption obj)
        {
            if (CompletedOptionsParent != null)
                obj.transform.SetParent(CompletedOptionsParent);
            PanelCompletedHandler.SetCompleted();
        }

        protected virtual BaseReaderDialogueOption GetChildPanelPrefab(UserPanel childPanel)
        {
            var type = childPanel.Data.Type;

            if (PanelOptions.Count == 1)
                return PanelOptions[0];

            foreach (var panelOption in PanelOptions) {
                if (string.Equals(type, panelOption.Type, StringComparison.OrdinalIgnoreCase))
                    return panelOption;
            }

            Debug.LogError($"No prefab for panel type \"{type}\"");
            return null;
        }
    }
}
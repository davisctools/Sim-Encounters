using ClinicalTools.Collections;
using ClinicalTools.UI;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ReaderQuizPanelsCreator : BaseChildUserPanelsDrawer
    {
        public BaseReaderPanelBehaviour MultipleChoicePanel { get => multipleChoicePanel; set => multipleChoicePanel = value; }
        [SerializeField] private BaseReaderPanelBehaviour multipleChoicePanel;
        public BaseReaderPanelBehaviour CheckBoxPanel { get => checkBoxPanel; set => checkBoxPanel = value; }
        [SerializeField] private BaseReaderPanelBehaviour checkBoxPanel;

        protected BaseReaderPanelBehaviour.Factory ReaderPanelFactory { get; set; }
        [Inject] public virtual void Inject(BaseReaderPanelBehaviour.Factory readerPanelFactory) => ReaderPanelFactory = readerPanelFactory;

        protected OrderedCollection<UserPanel> CurrentPanels { get; set; }
        protected bool IsActive { get; set; }
        protected Dictionary<UserPanel, BaseReaderPanelBehaviour> Children { get; } = new Dictionary<UserPanel, BaseReaderPanelBehaviour>();
        public override void Display(OrderedCollection<UserPanel> panels, bool active)
        {
            if (CurrentPanels == panels) {
                HandleSamePanels(panels, active);
                return;
            }

            CurrentPanels = panels;
            IsActive = active;

            foreach (var child in Children)
                Destroy(child.Value.gameObject);
            Children.Clear();

            foreach (var childPanel in panels.Values) {
                var prefab = GetChildPanelPrefab(childPanel);
                if (prefab == null)
                    continue;

                var panel = ReaderPanelFactory.Create(prefab);
                panel.transform.SetParent(transform);
                panel.transform.localScale = Vector3.one;
                panel.Select(this, new UserPanelSelectedEventArgs(childPanel, active));
                Children.Add(childPanel, panel);
            }
        }

        protected virtual void HandleSamePanels(OrderedCollection<UserPanel> panels, bool active)
        {
            if (IsActive == active)
                return;
            IsActive = active;
            if (IsActive)
                NextFrame.Function(SelectChildren);
        }

        protected virtual void SelectChildren()
        {
            foreach (var child in Children)
                child.Value.Select(this, new UserPanelSelectedEventArgs(child.Key, IsActive));
        }

        protected virtual BaseReaderPanelBehaviour GetChildPanelPrefab(UserPanel panel)
        {
            if (panel.Data.Values.ContainsKey("OptionTypeValue") && panel.Data.Values["OptionTypeValue"] == "Multiple Choice")
                return MultipleChoicePanel;
            else
                return CheckBoxPanel;
        }
    }
}
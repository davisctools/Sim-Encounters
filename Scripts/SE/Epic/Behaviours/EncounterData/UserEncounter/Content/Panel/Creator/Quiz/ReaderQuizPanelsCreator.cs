using ClinicalTools.Collections;
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

        protected List<BaseReaderPanelBehaviour> PanelBehaviours { get; } = new List<BaseReaderPanelBehaviour>();

        protected BaseReaderPanelBehaviour.Factory ReaderPanelFactory { get; set; }
        [Inject] public virtual void Inject(BaseReaderPanelBehaviour.Factory readerPanelFactory) => ReaderPanelFactory = readerPanelFactory;

        protected OrderedCollection<UserPanel> CurrentPanels { get; set; }
        public override void Display(OrderedCollection<UserPanel> panels, bool active)
        {
            if (CurrentPanels == panels)
                return;
            CurrentPanels = panels;

            foreach (var panelBehaviour in PanelBehaviours)
                Destroy(panelBehaviour.gameObject);
            PanelBehaviours.Clear();

            foreach (var childPanel in panels.Values) {
                var prefab = GetChildPanelPrefab(childPanel);
                if (prefab == null)
                    continue;

                var panel = ReaderPanelFactory.Create(prefab);
                panel.transform.SetParent(transform);
                panel.transform.localScale = Vector3.one;
                panel.Select(this, new UserPanelSelectedEventArgs(childPanel, active));
                PanelBehaviours.Add(panel);
            }
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
using ClinicalTools.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class WriterPresetTitledPanelsDrawer : BaseWriterPanelsDrawer
    {
        public Transform PanelsParent { get => panelsParent; set => panelsParent = value; }
        [SerializeField] private Transform panelsParent;
        public BaseWriterPanel PanelPrefab { get => panelPrefab; set => panelPrefab = value; }
        [SerializeField] private BaseWriterPanel panelPrefab;
        public List<string> PanelNames { get => panelNames; set => panelNames = value; }
        [SerializeField] private List<string> panelNames;

        protected BaseWriterPanel.Factory PanelFactory { get; set; }
        [Inject] public virtual void Inject(BaseWriterPanel.Factory panelFactory) => PanelFactory = panelFactory;
        
        protected virtual OrderedCollection<BaseWriterPanel> WriterPanels { get; set; } = new OrderedCollection<BaseWriterPanel>();

        public override void DrawChildPanels(OrderedCollection<Panel> childPanels)
        {
            foreach (var writerPanel in WriterPanels.Values)
                Destroy(writerPanel.gameObject);

            WriterPanels = new OrderedCollection<BaseWriterPanel>();

            var panels = new List<BaseWriterPanel>();
            foreach (var panel in childPanels) {
                var panelUI = InstantiatePanel();
                panelUI.Select(this, new PanelSelectedEventArgs(panel.Value));
                panels.Add(panelUI);
                WriterPanels.Add(panel.Key, panelUI);
            }
        }

        public override void DrawDefaultChildPanels()
        {
            WriterPanels = new OrderedCollection<BaseWriterPanel>();

            var panels = new List<BaseWriterPanel>();
            foreach (var panelName in PanelNames) {
                var panel = new Panel(PanelPrefab.Type);
                panel.LegacyValues.Add("PanelNameValue", panelName);
                var panelUI = InstantiatePanel();
                panelUI.Select(this, new PanelSelectedEventArgs(panel));
                panels.Add(panelUI);
                WriterPanels.Add(panelUI);
            }
        }

        protected virtual BaseWriterPanel InstantiatePanel()
        {
            var writerPanel = PanelFactory.Create(PanelPrefab);
            writerPanel.transform.SetParent(PanelsParent);
            writerPanel.transform.localScale = Vector3.one;
            return writerPanel;
        }

        public override OrderedCollection<Panel> SerializeChildren()
        {
            var blah = new ChildrenPanelManager();
            return blah.SerializeChildren(WriterPanels);
        }

    }
}
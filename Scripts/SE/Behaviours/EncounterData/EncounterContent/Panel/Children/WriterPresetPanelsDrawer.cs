using ClinicalTools.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class WriterPresetPanelsDrawer : BaseWriterPanelsDrawer
    {
        public Transform PanelsParent { get => panelsParent; set => panelsParent = value; }
        [SerializeField] private Transform panelsParent;
        public List<BaseWriterPanel> PresetPanels { get => presetPanels; set => presetPanels = value; }
        [SerializeField] private List<BaseWriterPanel> presetPanels;

        protected BaseWriterPanel.Factory PanelFactory { get; set; }
        [Inject] public virtual void Inject(BaseWriterPanel.Factory panelFactory) => PanelFactory = panelFactory;


        protected virtual OrderedCollection<BaseWriterPanel> WriterPanels { get; set; } = new OrderedCollection<BaseWriterPanel>();
        public override void DrawChildPanels(OrderedCollection<Panel> childPanels)
        {
            foreach (var writerPanel in WriterPanels.Values)
                Destroy(writerPanel.gameObject);

            var childrenPanelManager = new ChildrenPanelManager();
            WriterPanels = new OrderedCollection<BaseWriterPanel>();

            var panels = new List<BaseWriterPanel>();
            foreach (var panel in childPanels) {
                var prefab = childrenPanelManager.ChoosePrefab(PresetPanels, panel.Value);
                var panelUI = InstantiatePanel(prefab);
                panelUI.Display(this, new PanelSelectedEventArgs(panel.Value));
                panels.Add(panelUI);
                WriterPanels.Add(panel.Key, panelUI);
            }
        }

        public override void DrawDefaultChildPanels()
        {
            foreach (var writerPanel in WriterPanels.Values)
                Destroy(writerPanel.gameObject);

            WriterPanels = new OrderedCollection<BaseWriterPanel>();

            var panels = new List<BaseWriterPanel>();
            foreach (var panelPrefab in PresetPanels) {
                var panelUI = InstantiatePanel(panelPrefab);
                panelUI.Display(this, new PanelSelectedEventArgs(null));
                panels.Add(panelUI);
                WriterPanels.Add(panelUI);
            }
        }
        protected virtual BaseWriterPanel InstantiatePanel(BaseWriterPanel writerPanelPrefab)
        {
            var writerPanel = PanelFactory.Create(writerPanelPrefab);
            writerPanel.transform.SetParent(PanelsParent);
            writerPanel.transform.localScale = Vector3.one;
            return writerPanel;
        }

        public override OrderedCollection<Panel> SerializeChildren()
        {
            var childrenPanelManager = new ChildrenPanelManager();
            return childrenPanelManager.SerializeChildren(WriterPanels);
        }
    }
}
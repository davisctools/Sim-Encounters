using ClinicalTools.Collections;
using System;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public class ChildrenPanelManager
    {
        public virtual T ChoosePrefab<T>(List<T> panelOptions, Panel panel) where T : BaseWriterPanel
        {
            if (panelOptions.Count == 1)
                return panelOptions[0];

            foreach (var option in panelOptions) {
                if (string.Equals(option.Type, panel.Type, StringComparison.InvariantCultureIgnoreCase))
                    return option;
            }

            return panelOptions[0];
        }

        public virtual OrderedCollection<Panel> SerializeChildren(OrderedCollection<BaseWriterPanel> writerPanels)
        {
            List<KeyValuePair<string, BaseWriterPanel>> nonNullPanels = new List<KeyValuePair<string, BaseWriterPanel>>();
            foreach (var panel in writerPanels) {
                if (panel.Value != null)
                    nonNullPanels.Add(panel);
            }

            var panelsArr = new KeyValuePair<string, BaseWriterPanel>[nonNullPanels.Count];
            foreach (var panel in nonNullPanels) {
                var index = panel.Value.transform.GetSiblingIndex();
                panelsArr[index] = panel;
            }

            var panels = new OrderedCollection<Panel>();
            foreach (var writerPanel in panelsArr) {
                var panel = writerPanel.Value.Serialize();
                if (writerPanel.Key == null)
                    panels.Add(panel);
                else
                    panels.Add(writerPanel.Key, panel);
            }
            return panels;
        }
    }
}
using TMPro;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class WriterLabelPanelField : BaseWriterPanelField
    {
        public override string Value => Label.text;

        private TextMeshProUGUI label;
        protected TextMeshProUGUI Label {
            get {
                if (label == null)
                    label = GetComponent<TextMeshProUGUI>();
                return label;
            }
        }

        protected override void OnPanelSelected(object sender, PanelSelectedEventArgs e)
        {
            var values = e.Panel.Values;
            Label.text = values.ContainsKey(Name) ? values[Name] : "";
        }
    }
}
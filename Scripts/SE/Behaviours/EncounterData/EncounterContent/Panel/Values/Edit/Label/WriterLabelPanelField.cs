using TMPro;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class WriterLabelPanelField : BaseWriterPanelField
    {
        public override string Value => Label.text;

        protected TextMeshProUGUI Label => (label == null) ? label = GetComponent<TextMeshProUGUI>() : label;
        private TextMeshProUGUI label;

        protected override void OnPanelSelected(object sender, PanelSelectedEventArgs e)
        {
            var values = e.Panel.Values;
            Label.text = values.ContainsKey(Name) ? values[Name] : "";
        }
    }
}
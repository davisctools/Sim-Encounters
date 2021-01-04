using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    [RequireComponent(typeof(Toggle))]
    public class WriterTogglePanelField : BaseWriterPanelField
    {
        public override string Value => Toggle.isOn ? true.ToString() : null;

        protected Toggle Toggle => (toggle == null) ? toggle = GetComponent<Toggle>() : toggle;
        private Toggle toggle;

        protected override void OnPanelSelected(object sender, PanelSelectedEventArgs e)
        {
            var values = e.Panel.Values;
            if (values.ContainsKey(Name) && bool.TryParse(values[Name], out var boolVal))
                Toggle.isOn = boolVal;
        }
    }
}
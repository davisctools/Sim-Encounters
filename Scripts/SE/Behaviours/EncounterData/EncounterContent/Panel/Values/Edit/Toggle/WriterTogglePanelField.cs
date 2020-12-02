using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    [RequireComponent(typeof(Toggle))]
    public class WriterTogglePanelField : BaseWriterPanelField
    {
        public override string Value => Toggle.isOn ? true.ToString() : null;

        private Toggle toggle;
        protected Toggle Toggle {
            get {
                if (toggle == null)
                    toggle = GetComponent<Toggle>();
                return toggle;
            }
        }

        protected override void OnPanelSelected(object sender, PanelSelectedEventArgs e)
        {
            var values = e.Panel.Values;
            if (values.ContainsKey(Name) && bool.TryParse(values[Name], out var boolVal))
                Toggle.isOn = boolVal;
        }
    }
}
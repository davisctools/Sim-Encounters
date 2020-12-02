using TMPro;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    [RequireComponent(typeof(TMP_InputField))]
    public class WriterTextPanelField : BaseWriterPanelField
    {
        public override string Value => InputField.text;

        private TMP_InputField inputField;
        protected TMP_InputField InputField {
            get {
                if (inputField == null)
                    inputField = GetComponent<TMP_InputField>();
                return inputField;
            }
        }

        protected override void OnPanelSelected(object sender, PanelSelectedEventArgs e)
        {
            var values = e.Panel.Values;
            InputField.text = values.ContainsKey(Name) ? values[Name] : "";
        }
    }
}
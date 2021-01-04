using ClinicalTools.UI;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    [RequireComponent(typeof(FormattedInputField))]
    public class WriterTextPanelField : BaseWriterPanelField
    {
        public override string Value => InputField.GetUnformattedText();

        protected FormattedInputField InputField
            => (inputField == null) ? inputField = GetComponent<FormattedInputField>() : inputField;
        private FormattedInputField inputField;

        protected override void OnPanelSelected(object sender, PanelSelectedEventArgs e)
        {
            var values = e.Panel.Values;
            InputField.SetUnformattedText(values.ContainsKey(Name) ? values[Name] : "");
        }
    }
}
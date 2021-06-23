using ClinicalTools.UI;
using TMPro;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    [RequireComponent(typeof(TMP_InputField))]
    public class WriterTextPanelField : BaseWriterPanelField
    {
        public override string Value {
            get => InputField is FormattedInputField formattedInputField
                ? formattedInputField.GetUnformattedText()
                : InputField.text;
        }

        protected TMP_InputField InputField => (inputField == null) ? inputField = GetComponent<TMP_InputField>() : inputField;
        private TMP_InputField inputField;

        protected override void OnPanelSelected(object sender, PanelSelectedEventArgs e)
        {
            var values = e.Panel.Values;
            string value = values.ContainsKey(Name) ? values[Name] : "";

            if (InputField is FormattedInputField formattedInputField)
                formattedInputField.SetUnformattedText(value);
            else
                InputField.text = value;
        }
    }
}
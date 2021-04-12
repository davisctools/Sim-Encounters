namespace ClinicalTools.SimEncounters
{
    public class WriterHiddenPanelField : BaseWriterPanelField
    {
        public override string Value { get => value; }
        private string value;

        protected override void OnPanelSelected(object sender, PanelSelectedEventArgs e)
        {
            var values = e.Panel.LegacyValues;
            if (values.ContainsKey(Name))
                value = values[Name];
        }
    }
}
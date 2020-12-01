namespace ClinicalTools.SimEncounters
{
    public class UrlArgument
    {
        public string Name { get; }
        public string Value { get; }

        public UrlArgument(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}
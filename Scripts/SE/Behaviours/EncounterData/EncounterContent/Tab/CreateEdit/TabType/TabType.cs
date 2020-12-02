namespace ClinicalTools.SimEncounters
{
    public class TabType
    {
        public string Display { get; }
        public string Prefab { get; }
        public virtual string Description { get; }

        public TabType(string display, string prefab, string description)
        {
            Display = display;
            Prefab = prefab;
            Description = $"<b>{display}</b>\n{description}";
        }
    }
}
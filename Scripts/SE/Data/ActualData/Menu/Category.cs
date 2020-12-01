using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public class Category
    {
        public string Name { get; }
        public List<MenuEncounter> Encounters { get; } = new List<MenuEncounter>();

        public Category(string name)
        {
            Name = name;
        }

        public bool IsCompleted()
        {
            foreach (var encounter in Encounters)
                if (encounter.Status == null || !encounter.Status.Completed)
                    return false;

            return true;
        }
    }
}
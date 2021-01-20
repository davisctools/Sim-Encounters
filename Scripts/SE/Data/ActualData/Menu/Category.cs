using System;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public class Category
    {
        public string Name { get; }
        protected List<MenuEncounter> Encounters { get; } = new List<MenuEncounter>();

        public event Action EncountersChanged;

        public Category(string name)
        {
            Name = name;
        }


        public IEnumerable<MenuEncounter> GetEncounters() => Encounters;

        public void AddEncounter(MenuEncounter encounter)
        {
            Encounters.Add(encounter);
            EncountersChanged?.Invoke();
        }
        public bool ContainsEncounter(MenuEncounter encounter) => Encounters.Contains(encounter);
        public void RemoveEncounter(MenuEncounter encounter)
        {
            Encounters.Remove(encounter);
            EncountersChanged?.Invoke();
        }
        public int EncounterCount => Encounters.Count;

        public bool IsCompleted()
        {
            foreach (var encounter in Encounters)
                if (encounter.Status == null || !encounter.Status.Completed)
                    return false;

            return true;
        }
    }
}
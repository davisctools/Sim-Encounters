
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public interface IMenuEncountersInfo
    {
        IEnumerable<Category> GetCategories();
        IEnumerable<MenuEncounter> GetEncounters();
        IEnumerable<MenuEncounter> GetUserEncounters();
        IEnumerable<MenuEncounter> GetTemplates();
    }
}
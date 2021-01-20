
using System;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public interface IMenuEncountersInfo
    {
        IEnumerable<Category> GetCategories();
        IEnumerable<MenuEncounter> GetEncounters();
        IEnumerable<MenuEncounter> GetUserEncounters();
        IEnumerable<MenuEncounter> GetTemplates();
        void RemoveEncounter(MenuEncounter encounter);

        event Action EncountersChanged;
        event Action UserEncountersChanged;
        event Action TemplatesChanged;
        event Action CategoriesChanged;
    }
}
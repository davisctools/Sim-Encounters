﻿
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public class MenuEncountersInfo : IMenuEncountersInfo
    {
        protected virtual HashSet<MenuEncounter> Encounters { get; } = new HashSet<MenuEncounter>();
        protected virtual HashSet<MenuEncounter> UserEncounters { get; } = new HashSet<MenuEncounter>();
        protected virtual HashSet<MenuEncounter> Templates { get; } = new HashSet<MenuEncounter>();
        protected virtual Dictionary<string, Category> Categories { get; } = new Dictionary<string, Category>();

        protected User User { get;  }
        public MenuEncountersInfo(User user) => User = user;

        public virtual void AddEncounter(MenuEncounter encounter)
        {
            if (Encounters.Contains(encounter) || Templates.Contains(encounter))
                return;

            var metadata = encounter.GetLatestMetadata();

            if (metadata.IsTemplate) {
                Templates.Add(encounter);
                return;
            }

            Encounters.Add(encounter);
            if (metadata.AuthorAccountId == User.AccountId)
                UserEncounters.Add(encounter);
            foreach (var categoryName in metadata.Categories)
                AddToCategory(encounter, categoryName);
        }

        protected virtual void AddToCategory(MenuEncounter encounter, string categoryName)
        {
            if (string.IsNullOrWhiteSpace(categoryName))
                return;

            Category category;
            if (!Categories.ContainsKey(categoryName)) {
                category = new Category(categoryName);
                Categories.Add(categoryName, category);
            } else {
                category = Categories[categoryName];
            }
            category.Encounters.Add(encounter);
        }


        public IEnumerable<MenuEncounter> GetTemplates() => Templates;
        public IEnumerable<MenuEncounter> GetEncounters() => Encounters;
        public IEnumerable<MenuEncounter> GetUserEncounters() => UserEncounters;
        public IEnumerable<Category> GetCategories() => Categories.Values;
    }
}
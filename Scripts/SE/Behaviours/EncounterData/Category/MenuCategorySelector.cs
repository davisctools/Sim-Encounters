using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class MenuCategorySelector : BaseCategorySelector
    {
        public Transform OptionsParent { get => optionsParent; set => optionsParent = value; }
        [SerializeField] private Transform optionsParent;
        protected CategorySelector.Pool OptionPool { get; set; }

        protected List<CategorySelector> CategoryUIs { get; } = new List<CategorySelector>();
        protected IEnumerable<Category> CurrentCategories { get; set; }


        [Inject] protected virtual void Inject(CategorySelector.Pool optionPool) => OptionPool = optionPool;

        public override void Display(MenuSceneInfo sceneInfo, IEnumerable<Category> categories)
        {
            gameObject.SetActive(true);
            if (CurrentCategories == categories)
                return;
            CurrentCategories = categories;

            foreach (var categoryUI in CategoryUIs) {
                categoryUI.Selected -= Select;
                OptionPool.Despawn(categoryUI);
            }
            CategoryUIs.Clear();

            var sortedCategories = SortCategories(categories);
            foreach (var category in sortedCategories)
                ShowCategoryButton(category);
        }

        protected virtual IEnumerable<Category> SortCategories(IEnumerable<Category> categories)
        {
            var sortedCategories = new List<Category>(categories);
            sortedCategories.Sort((c1, c2) => c1.Name.CompareTo(c2.Name));
            return sortedCategories;
        }

        protected virtual void ShowCategoryButton(Category category)
        {
            var categoryUI = OptionPool.Spawn(); 
            categoryUI.transform.SetParent(OptionsParent);
            categoryUI.transform.localScale = Vector3.one;
            categoryUI.Select(this, new CategorySelectedEventArgs(category));
            categoryUI.Selected += Select;
            CategoryUIs.Add(categoryUI);
        }

        public override void Show() => gameObject.SetActive(true);

        public override void Hide() => gameObject.SetActive(false);
    }
}
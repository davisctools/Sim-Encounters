using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class ReaderMenuEncountersUI : BaseMenuSceneDrawer
    {
        public bool SelectCategory { get => selectCategory; set => selectCategory = value; }
        [SerializeField] private bool selectCategory;

        public BaseCategorySelector CategorySelector { get => categorySelector; set => categorySelector = value; }
        [SerializeField] private BaseCategorySelector categorySelector;
        public BaseMenuEncounterSelectionManager EncounterSelector { get => encounterSelector; set => encounterSelector = value; }
        [SerializeField] private BaseMenuEncounterSelectionManager encounterSelector;
        public ChangeSidePanelScript ShowCategoriesToggle { get => showCategoriesToggle; set => showCategoriesToggle = value; }
        [SerializeField] private ChangeSidePanelScript showCategoriesToggle;
        public ChangeSidePanelScript ShowEncountersToggle { get => showEncountersToggle; set => showEncountersToggle = value; }
        [SerializeField] private ChangeSidePanelScript showEncountersToggle;

        public List<GameObject> DownloadingMessageObjects { get => downloadingMessageObjects; set => downloadingMessageObjects = value; }
        [SerializeField] private List<GameObject> downloadingMessageObjects;

        public MenuSceneInfo SceneInfo { get; set; }

        protected virtual bool IsOn { get; set; }
        protected virtual void AddListeners()
        {
            if (IsOn)
                return;
            IsOn = true;

            if (!SelectCategory)
                return;

            ShowCategoriesToggle.Selected += DisplayCategories;

            CategorySelector.Selected += CategorySelected;
            if (CategorySelector.CurrentValue != null)
                CategorySelected(CategorySelector, CategorySelector.CurrentValue);
        }
        protected virtual void RemoveListeners()
        {
            if (!IsOn)
                return;
            IsOn = false;

            ShowCategoriesToggle.Selected -= DisplayCategories;
            CategorySelector.Selected -= CategorySelected;
        }

        public override void Display(LoadingMenuSceneInfo loadingSceneInfo)
        {
            AddListeners();

            EncounterSelector.Initialize();
            foreach (var downloadingMessageObject in DownloadingMessageObjects)
                downloadingMessageObject.SetActive(true);

            if (SelectCategory) {
                ShowCategoriesToggle.Display();
                ShowCategoriesToggle.Select();
                DisplayCategories();
            } else {
                if (ShowCategoriesToggle != null)
                    ShowCategoriesToggle.Hide();
                if (ShowEncountersToggle != null)
                    ShowEncountersToggle.Hide();
                if (CategorySelector != null)
                    CategorySelector.Hide();
            }

            loadingSceneInfo.Result.AddOnCompletedListener(SceneInfoLoaded);
        }

        protected virtual void SceneInfoLoaded(TaskResult<MenuSceneInfo> sceneInfo)
        {
            SceneInfo = sceneInfo.Value;

            DisplayOnUpdate = true;
        }

        protected bool DisplayOnUpdate { get; set; }
        protected virtual void Update()
        {
            if (!DisplayOnUpdate)
                return;

            DisplayOnUpdate = false;
            SceneInfo.LoadingScreen?.Stop();
            foreach (var downloadingMessageObject in DownloadingMessageObjects)
                downloadingMessageObject.SetActive(false);

            if (SelectCategory)
                CategorySelector.Display(SceneInfo, SceneInfo.MenuEncountersInfo.GetCategories());
            else
                EncounterSelector.DisplayForRead(SceneInfo, SceneInfo.MenuEncountersInfo.GetEncounters());
        }

        private void DisplayCategories()
        {
            ShowEncountersToggle.Hide();
            EncounterSelector.Hide();

            CategorySelector.Show();
        }

        protected virtual void CategorySelected(object sender, CategorySelectedEventArgs eventArgs)
        {
            CategorySelector.Hide();

            ShowEncountersToggle.Show(eventArgs.Category.Name);
            EncounterSelector.DisplayForRead(SceneInfo, eventArgs.Category.GetEncounters());
        }

        public override void Hide()
        {
            RemoveListeners();

            CategorySelector.Hide();
            EncounterSelector.Hide();
            ShowCategoriesToggle.Hide();
            ShowEncountersToggle.Hide();
        }
    }
}
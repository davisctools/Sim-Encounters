using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class WriterMenuEncountersUI : BaseMenuSceneDrawer
    {
        public BaseMenuEncounterSelectionManager EncounterSelector { get => encounterSelector; set => encounterSelector = value; }
        [SerializeField] private BaseMenuEncounterSelectionManager encounterSelector;
        public GameObject DownloadingCases { get => downloadingCases; set => downloadingCases = value; }
        [SerializeField] private GameObject downloadingCases;
        public ChangeSidePanelScript ShowEncountersToggle { get => showEncountersToggle; set => showEncountersToggle = value; }
        [SerializeField] private ChangeSidePanelScript showEncountersToggle;
        public ChangeSidePanelScript ShowTemplatesToggle { get => showTemplatesToggle; set => showTemplatesToggle = value; }
        [SerializeField] private ChangeSidePanelScript showTemplatesToggle;
        public MenuEncounterOverview Overview { get => overview; set => overview = value; }
        [SerializeField] private MenuEncounterOverview overview;

        public MenuSceneInfo SceneInfo { get; set; }

        protected virtual bool IsOn { get; set; }
        protected virtual void AddListeners()
        {
            if (IsOn)
                return;
            IsOn = true;

            ShowEncountersToggle.Selected += DisplayEncounters;
            ShowTemplatesToggle.Selected += DisplayTemplates;
        }
        protected virtual void RemoveListeners()
        {
            if (!IsOn)
                return;
            IsOn = false;

            ShowEncountersToggle.Selected -= DisplayEncounters;
            ShowTemplatesToggle.Selected -= DisplayTemplates;
        }

        public void Initialize()
        {
            AddListeners();
            ShowEncountersToggle.Select();
            DownloadingCases.SetActive(true);
            EncounterSelector.Initialize();
        }

        public override void Display(LoadingMenuSceneInfo loadingSceneInfo)
        {
            ShowTemplatesToggle.Display();
            ShowEncountersToggle.Display();
            SceneInfo = null;
            Initialize();
            ShowCasesLoading();
            loadingSceneInfo.Result.AddOnCompletedListener(ShowEncounters);
        }

        protected virtual void ShowCasesLoading() => DownloadingCases.SetActive(true);

        protected virtual void ShowEncounters(TaskResult<MenuSceneInfo> sceneInfo)
        {
            sceneInfo.Value.LoadingScreen?.Stop();
            DownloadingCases.SetActive(false);

            SceneInfo = sceneInfo.Value;

            if (ShowEncountersToggle.IsOn())
                DisplayEncounters();
            else
                DisplayTemplates();
        }

        protected virtual void DisplayEncounters()
        {
            if (SceneInfo != null)
                DisplayEncounters(SceneInfo.MenuEncountersInfo.GetUserEncounters());
        }
        protected virtual void DisplayTemplates()
        {
            if (SceneInfo != null) 
                DisplayEncounters(SceneInfo.MenuEncountersInfo.GetTemplates());
        }
        protected virtual void DisplayEncounters(IEnumerable<MenuEncounter> encounters)
                => EncounterSelector.DisplayForEdit(SceneInfo, encounters);

        public override void Hide()
        {
            RemoveListeners();
            EncounterSelector.Hide();
            ShowTemplatesToggle.Hide();
            ShowEncountersToggle.Hide();
        }
    }
}
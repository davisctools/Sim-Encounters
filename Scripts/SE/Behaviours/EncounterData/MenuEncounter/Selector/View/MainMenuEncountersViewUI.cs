using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class MainMenuEncountersViewUI : BaseViewEncounterSelector
    {
        public override string ViewName { get => viewName; set => viewName = value; }
        [SerializeField] private string viewName;
        public override Sprite ViewSprite { get => viewSprite; set => viewSprite = value; }
        [SerializeField] private Sprite viewSprite;
        public Button NewCaseButton { get => newCaseButton; set => newCaseButton = value; }
        [SerializeField] private Button newCaseButton;
        public Transform OptionsParent { get => optionsParent; set => optionsParent = value; }
        [SerializeField] private Transform optionsParent;
        protected MenuEncounterSelector.Pool OptionPool { get; set; }

        protected MenuSceneInfo CurrentSceneInfo { get; set; }

        protected virtual bool IsRead { get; set; }

        protected BaseAddEncounterPopup AddEncounterPopup { get; set; }
        [Inject]
        protected virtual void Inject(
            MenuEncounterSelector.Pool optionPool,
            BaseAddEncounterPopup addEncounterPopup)
        {
            OptionPool = optionPool;
            AddEncounterPopup = addEncounterPopup;
        }
        protected virtual void Awake()
        {
            if (NewCaseButton != null)
                NewCaseButton.onClick.AddListener(AddNewCase);
        }

        protected MenuSceneInfo SceneInfo { get; set; }
        public override void DisplayForRead(MenuSceneInfo sceneInfo, IEnumerable<MenuEncounter> encounters)
        {
            IsRead = true;
            TryToDisplay(sceneInfo, encounters);
        }

        public override void DisplayForEdit(MenuSceneInfo sceneInfo, IEnumerable<MenuEncounter> encounters)
        {
            IsRead = false;
            TryToDisplay(sceneInfo, encounters);
        }

        // This is a really messy solution, but othewise we hit an issue on mobile
        protected virtual void TryToDisplay(MenuSceneInfo sceneInfo, IEnumerable<MenuEncounter> encounters)
        {
            gameObject.SetActive(true);
            displayOnUpdate = true;
            SceneInfo = sceneInfo;
            UpdateEncounters = encounters;
        }

        protected IEnumerable<MenuEncounter> UpdateEncounters { get; set; }
        private bool displayOnUpdate;
        protected virtual void Update()
        {
            if (!displayOnUpdate || CanvasUpdateRegistry.IsRebuildingLayout())
                return;

            displayOnUpdate = false;

            Display(SceneInfo, UpdateEncounters);
        }

        protected IEnumerable<MenuEncounter> CurrentEncounters { get; set; }
        protected virtual void Display(MenuSceneInfo sceneInfo, IEnumerable<MenuEncounter> encounters)
        {
            SceneInfo = sceneInfo;
            if (NewCaseButton != null)
                NewCaseButton.gameObject.SetActive(!IsRead);
            gameObject.SetActive(true);
            CurrentSceneInfo = sceneInfo;

            if (CurrentEncounters == encounters)
                return;
            CurrentEncounters = encounters;

            foreach (MenuEncounterSelector encounterDisplay in EncounterDisplays)
                OptionPool.Despawn(encounterDisplay);

            EncounterDisplays.Clear();

            foreach (var encounter in encounters)
                SetEncounter(encounter);
        }


        public override void Hide()
        {
            gameObject.SetActive(false);
        }

        protected List<MenuEncounterSelector> EncounterDisplays { get; } = new List<MenuEncounterSelector>();
        protected virtual void SetEncounter(MenuEncounter encounter)
        {
            var encounterUI = OptionPool.Spawn();
            encounterUI.transform.SetParent(OptionsParent);
            encounterUI.transform.SetAsLastSibling();
            encounterUI.transform.localScale = Vector3.one;

            if (IsRead)
                encounterUI.Select(this, new MenuEncounterSelectedEventArgs(encounter, EncounterSelectionType.Read));
            else
                encounterUI.Select(this, new MenuEncounterSelectedEventArgs(encounter, EncounterSelectionType.Edit));

            EncounterDisplays.Add(encounterUI);
        }

        public override void Initialize() { }

        protected virtual void AddNewCase() => AddEncounterPopup.Display(SceneInfo);
    }
}
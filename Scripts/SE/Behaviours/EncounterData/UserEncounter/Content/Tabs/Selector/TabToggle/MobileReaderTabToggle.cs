using ClinicalTools.UI;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class MobileReaderTabToggle : BaseReaderTabToggle
    {
        public SelectableToggle SelectToggle { get => selectToggle; set => selectToggle = value; }
        [SerializeField] private SelectableToggle selectToggle;
        public Image SelectedImage { get => selectedImage; set => selectedImage = value; }
        [SerializeField] private Image selectedImage;
        public GameObject VisitedCheck { get => visitedCheck; set => visitedCheck = value; }
        [SerializeField] private GameObject visitedCheck;

        protected IColorManager ColorManager { get; set; }
        protected SignalBus SignalBus { get; set; }
        [Inject]
        public virtual void Inject(SignalBus signalBus, IColorManager colorManager)
        {
            SignalBus = signalBus;
            SignalBus.Subscribe<EncounterCompletedSignal>(CompletionDraw); 
            ColorManager = colorManager;
        }

        public override event Action Selected;

        protected virtual void OnEnable() => StartCoroutine(AddSelectionListenerAfterDelay());
        protected virtual void OnDisable() => SelectToggle.Selected -= () => Selected?.Invoke();

        protected virtual IEnumerator AddSelectionListenerAfterDelay()
        {
            yield return null;
            SelectToggle.Selected += () => Selected?.Invoke();
        }

        private bool initialized;
        protected UserTab CurrentTab { get; set; }
        protected Color NotVisitedColor { get; set; }
        public override void Display(UserTab tab)
        {
            Selected = null;

            if (CurrentTab == tab) {
                UpdateIsVisited();
                return;
            }

            CurrentTab = tab;

            if (!initialized)
                Initialize();

            UpdateIsVisited();
        }

        protected virtual void Initialize()
        {
            initialized = true;
            NotVisitedColor = SelectToggle.Toggle.image.color;
            SelectToggle.Unselected += ToggleUnselected;
            SelectToggle.Selected += ToggleSelected;
        }

        public override void SetToggleGroup(ToggleGroup group) => SelectToggle.SetToggleGroup(group);

        protected virtual void UpdateIsVisited()
        {
            var visited = CurrentTab?.IsRead() == true;

            VisitedCheck.SetActive(visited);
            SelectToggle.Toggle.image.color = visited ? ColorManager.GetColor(ColorType.Green) : NotVisitedColor;
        }

        protected virtual void ToggleUnselected()
        {
            SelectedImage.gameObject.SetActive(false);
            if (CurrentTab.IsRead())
                UpdateIsVisited();
        }
        protected virtual void ToggleSelected()
        {
            SelectedImage.gameObject.SetActive(true);
            if (CurrentTab.IsRead())
                UpdateIsVisited();
        }

        public override void Select()
        {
            SelectToggle.Select();
            UpdateIsVisited();
        }

        protected virtual void CompletionDraw() => UpdateIsVisited();
    }
}
using ClinicalTools.SEColors;
using ClinicalTools.UI;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ReaderCircleTabToggle : BaseSelectableTabToggle
    {
        public SelectableToggle SelectToggle { get => selectToggle; set => selectToggle = value; }
        [SerializeField] private SelectableToggle selectToggle;
        public Image SelectedImage { get => selectedImage; set => selectedImage = value; }
        [SerializeField] private Image selectedImage;
        public GameObject VisitedCheck { get => visitedCheck; set => visitedCheck = value; }
        [SerializeField] private GameObject visitedCheck;

        protected virtual IColorManager ColorManager { get; } = new ColorManager();
        protected SignalBus SignalBus { get; set; }
        [Inject]
        public virtual void Inject(SignalBus signalBus)
        {
            SignalBus = signalBus;
            SignalBus.Subscribe<EncounterCompletedSignal>(CompletionDraw);
        }

        protected virtual void OnEnable() => StartCoroutine(AddSelectionListenerAfterDelay());
        protected virtual void OnDisable() => SelectToggle.Selected -= Selected;

        protected virtual IEnumerator AddSelectionListenerAfterDelay()
        {
            yield return null;
            SelectToggle.Selected += Selected;
        }

        protected virtual void Selected()
        {
            if (TabSelector.CurrentValue.SelectedTab != Tab)
                TabSelector.Select(this, new UserTabSelectedEventArgs(Tab, ChangeType.MoveTo));
        }

        private bool initialized;
        protected Color NotVisitedColor { get; set; }
        public override void Initialize(UserTab tab)
        {
            base.Initialize(tab);

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
            var visited = Tab?.IsRead() == true;

            VisitedCheck.SetActive(visited);
            SelectToggle.Toggle.image.color = visited ? ColorManager.GetColor(ColorType.Green) : NotVisitedColor;
        }

        protected virtual void ToggleUnselected()
        {
            SelectedImage.gameObject.SetActive(false);
            if (Tab.IsRead())
                UpdateIsVisited();
        }
        protected virtual void ToggleSelected()
        {
            SelectedImage.gameObject.SetActive(true);
            if (Tab.IsRead())
                UpdateIsVisited();
        }

        public override void Select()
        {
            SelectToggle.Select();
            UpdateIsVisited();
        }

        public override void Deselect()
        {
            SelectToggle.DeselectWithNoNotify();
            ToggleUnselected();
        }

        protected virtual void CompletionDraw() => UpdateIsVisited();
    }
}
using ClinicalTools.SEColors;
using ClinicalTools.UI;
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

        protected virtual IColorManager ColorManager { get; } = new ColorManager();
        protected ILinearEncounterNavigator LinearEncounterNavigator { get; set; }
        protected SignalBus SignalBus { get; set; }
        [Inject]
        public virtual void Inject(ILinearEncounterNavigator linearEncounterNavigator, SignalBus signalBus)
        {
            LinearEncounterNavigator = linearEncounterNavigator;
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
        protected Color VisitedColor { get; set; } = new Color(1, 1, 1, .95f);
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
            LinearEncounterNavigator.EncounterTabPositionChanged += EncounterTabPositionChanged;
        }

        protected virtual void EncounterTabPositionChanged(object sender, UserTabSelectedEventArgs e)
        {
            if (e.SelectedTab != Tab)
                UpdateIsVisited();
        }

        public override void SetToggleGroup(ToggleGroup group) => SelectToggle.SetToggleGroup(group);


        protected virtual void UpdateIsVisited()
            => SelectToggle.Toggle.image.color = Tab?.IsRead() == true ? VisitedColor : NotVisitedColor;

        protected virtual void ToggleSelected() => SelectedImage.gameObject.SetActive(true);
        protected virtual void ToggleUnselected() => SelectedImage.gameObject.SetActive(false);

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
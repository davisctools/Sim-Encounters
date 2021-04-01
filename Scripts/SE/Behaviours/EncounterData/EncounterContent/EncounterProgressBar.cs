using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class EncounterProgressBar : MonoBehaviour
    {
        public RectTransform SectionDotPrefab { get => sectionDotPrefab; set => sectionDotPrefab = value; }
        [SerializeField] private RectTransform sectionDotPrefab;
        public Image FillImage { get => fillImage; set => fillImage = value; }
        [SerializeField] private Image fillImage;

        protected ISelectedListener<EncounterSelectedEventArgs> EncounterSelector { get; set; }
        protected ISelectedListener<TabSelectedEventArgs> TabSelector { get; set; }
        protected RectTransformFactory RectTransformFactory { get; set; }
        [Inject]
        public virtual void Inject(
            ISelectedListener<EncounterSelectedEventArgs> encounterSelector,
            ISelectedListener<TabSelectedEventArgs> tabSelector,
            RectTransformFactory rectTransformFactory)
        {
            EncounterSelector = encounterSelector;
            TabSelector = tabSelector;
            RectTransformFactory = rectTransformFactory;
        }
        protected virtual void Start()
        {
            EncounterSelector.Selected += OnEncounterSelected;
            if (EncounterSelector.CurrentValue != null)
                OnEncounterSelected(EncounterSelector, EncounterSelector.CurrentValue);

            TabSelector.Selected += OnTabSelected;
            if (TabSelector.CurrentValue != null)
                OnTabSelected(TabSelector, TabSelector.CurrentValue);
        }

        protected EncounterContent Content { get; set; }
        private int tabCount;
        public virtual void OnEncounterSelected(object sender, EncounterSelectedEventArgs eventArgs)
        {
            Content = eventArgs.Encounter.Content;
            var sections = Content.Sections;

            tabCount = Content.GetTabCount();
            var tabs = 0;

            var rect = ((RectTransform)transform).rect;

            for (var i = 0; i < sections.Count; i++) {
                CreateSectionDot(tabs, tabCount, rect.height);
                tabs += sections[i].Value.Tabs.Count;
            }
        }

        protected virtual void CreateSectionDot(int tabsSoFar, int tabCount, float height)
        {
            var sectionDot = RectTransformFactory.Create(sectionDotPrefab);
            sectionDot.SetParent(transform);
            sectionDot.localScale = Vector3.one;

            var position = 1f * tabsSoFar / tabCount;
            sectionDot.anchorMin = new Vector2(position, 0);
            sectionDot.anchorMax = new Vector2(position, 1);
            sectionDot.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);

            sectionDot.anchoredPosition = Vector2.zero;
        }

        protected virtual void OnTabSelected(object sender, TabSelectedEventArgs eventArgs)
            => FillImage.fillAmount = Content.GetCurrentTabNumber() / tabCount;

        protected virtual void OnDestroy()
        {
            EncounterSelector.Selected -= OnEncounterSelected;
            TabSelector.Selected -= OnTabSelected;
        }
    }
}

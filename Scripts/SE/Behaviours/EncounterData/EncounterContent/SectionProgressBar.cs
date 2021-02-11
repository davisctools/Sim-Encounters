using ClinicalTools.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class SectionProgressBar : MonoBehaviour
    {
        public RectTransform TabDotPrefab { get => tabDotPrefab; set => tabDotPrefab = value; }
        [SerializeField] private RectTransform tabDotPrefab;
        public Image FillImage { get => fillImage; set => fillImage = value; }
        [SerializeField] private Image fillImage;

        protected ISelectedListener<SectionSelectedEventArgs> SectionSelector { get; set; }
        protected ISelectedListener<TabSelectedEventArgs> TabSelector { get; set; }
        protected RectTransformFactory RectTransformFactory { get; set; }
        [Inject]
        public virtual void Inject(
            ISelectedListener<SectionSelectedEventArgs> sectionSelector,
            ISelectedListener<TabSelectedEventArgs> tabSelector,
            RectTransformFactory rectTransformFactory)
        {
            SectionSelector = sectionSelector;
            TabSelector = tabSelector;
            RectTransformFactory = rectTransformFactory;
        }
        protected virtual void Start()
        {
            SectionSelector.Selected += OnSectionSelected;
            if (SectionSelector.CurrentValue != null)
                OnSectionSelected(SectionSelector, SectionSelector.CurrentValue);

            TabSelector.Selected += OnTabSelected;
            if (TabSelector.CurrentValue != null)
                OnTabSelected(TabSelector, TabSelector.CurrentValue);
        }

        protected List<RectTransform> TabDots { get; } = new List<RectTransform>();
        protected OrderedCollection<Tab> Tabs { get; set; }
        public virtual void OnSectionSelected(object sender, SectionSelectedEventArgs eventArgs)
        {
            if (Tabs == eventArgs.SelectedSection.Tabs)
                return;

            foreach (var tabDot in TabDots)
                Destroy(tabDot.gameObject);
            TabDots.Clear();

            Tabs = eventArgs.SelectedSection.Tabs;

            var height = ((RectTransform)transform).rect.height;
            for (var i = 0; i < Tabs.Count; i++)
                CreateTabDot(i, height);
        }

        protected virtual void CreateTabDot(int tabNumber, float height)
        {
            var tabDot = RectTransformFactory.Create(TabDotPrefab);
            TabDots.Add(tabDot);
            tabDot.SetParent(transform);
            tabDot.localScale = Vector3.one;

            var position = 1f * tabNumber / Tabs.Count;
            tabDot.anchorMin = new Vector2(position, 0);
            tabDot.anchorMax = new Vector2(position, 1);
            tabDot.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);

            tabDot.anchoredPosition = Vector2.zero;
        }

        protected virtual void OnTabSelected(object sender, TabSelectedEventArgs eventArgs)
            => FillImage.fillAmount = (1f + Tabs.IndexOf(eventArgs.SelectedTab)) / Tabs.Count;

        protected virtual void OnDestroy()
        {
            SectionSelector.Selected -= OnSectionSelected;
            TabSelector.Selected -= OnTabSelected;
        }
    }
}

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class TableOfContentsSection : BaseTableOfContentsSection
    {
        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI label;
        [SerializeField] private Transform tabsParent;

        protected ISelector<UserSectionSelectedEventArgs> SectionSelector { get; set; }
        protected BaseTableOfContentsTab.Factory TabFactory { get; set; }
        [Inject]
        public virtual void Inject(
            ISelector<UserSectionSelectedEventArgs> sectionSelector,
            BaseTableOfContentsTab.Factory tabFactory)
        {
            SectionSelector = sectionSelector;
            TabFactory = tabFactory;
        }

        protected virtual void Start() => button.onClick.AddListener(SelectSection);

        protected UserSection Section { get; set; }
        public override void Display(UserSection section)
        {
            Section = section;
            label.text = section.Data.Name;
            label.color = section.Data.Color;
            DrawTabs(section.Tabs.Values);
        }

        protected virtual void DrawTabs(IEnumerable<UserTab> tabs)
        {
            foreach (var tab in tabs)
                DrawTab(tab);
        }

        protected virtual void DrawTab(UserTab tab)
        {
            var tableOfContentsTab = TabFactory.Create();
            tableOfContentsTab.transform.SetParent(tabsParent);
            tableOfContentsTab.transform.localScale = Vector3.one;
            tableOfContentsTab.Display(Section, tab);
        }


        protected virtual void SelectSection()
            => SectionSelector.Select(this, new UserSectionSelectedEventArgs(Section, ChangeType.JumpTo));
    }
}
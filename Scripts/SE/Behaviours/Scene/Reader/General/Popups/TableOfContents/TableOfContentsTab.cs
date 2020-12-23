using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class TableOfContentsTab : BaseTableOfContentsTab
    {
        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI label;

        protected ISelector<UserSectionSelectedEventArgs> SectionSelector { get; set; }
        protected ISelector<UserTabSelectedEventArgs> TabSelector { get; set; }
        [Inject] public virtual void Inject(
            ISelector<UserSectionSelectedEventArgs> sectionSelector,
            ISelector<UserTabSelectedEventArgs> tabSelector)
        {
            SectionSelector = sectionSelector;
            TabSelector = tabSelector;
        }

        protected virtual void Start() => button.onClick.AddListener(SelectTab);

        protected UserSection Section { get; set; }
        protected UserTab Tab { get; set; }
        public override void Display(UserSection section, UserTab tab)
        {
            Section = section;
            Tab = tab;
            label.text = tab.Data.Name;
            label.color = section.Data.Color;
        }

        protected virtual void SelectTab()
        {
            Section.Data.SetCurrentTab(Tab.Data);
            SectionSelector.Select(this, new UserSectionSelectedEventArgs(Section, ChangeType.JumpTo));
            TabSelector.Select(this, new UserTabSelectedEventArgs(Tab, ChangeType.JumpTo));
        }
    }
}
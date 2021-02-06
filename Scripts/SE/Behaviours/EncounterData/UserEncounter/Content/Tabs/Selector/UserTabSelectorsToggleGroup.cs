using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    public class UserTabSelectorsToggleGroup : UserTabSelectorsGroup<BaseSelectableTabToggle>
    {
        public virtual ToggleGroup TabsToggleGroup { get => tabsToggleGroup; set => tabsToggleGroup = value; }
        [SerializeField] private ToggleGroup tabsToggleGroup;

        protected override BaseSelectableTabToggle CreateNewTabButton()
        {
            var tabButton = base.CreateNewTabButton();
            tabButton.SetToggleGroup(TabsToggleGroup);
            return tabButton;
        }

        protected override void OnSelected(UserTab tab)
        {
            if (CurrentTab == tab)
                return;

            TabButtons[tab].Select();
            foreach (var tabButton in TabButtons.Where((b) => b.Key != tab))
                tabButton.Value.Deselect();

            base.OnSelected(tab);
        }
    }
}
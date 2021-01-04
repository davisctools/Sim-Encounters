using TMPro;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TabTitleLabel : MonoBehaviour
    {
        protected TextMeshProUGUI Label => (label == null) ? label = GetComponent<TextMeshProUGUI>() : label;
        private TextMeshProUGUI label;

        protected ISelectedListener<TabSelectedEventArgs> TabSelector { get; set; }
        [Inject]
        public virtual void Inject(ISelectedListener<TabSelectedEventArgs> tabSelector)
            => TabSelector = tabSelector;
        protected virtual void Start()
        {
            TabSelector.Selected += OnTabSelected;
            if (TabSelector.CurrentValue != null)
                OnTabSelected(TabSelector, TabSelector.CurrentValue);
        }
        protected virtual void OnTabSelected(object sender, TabSelectedEventArgs eventArgs)
            => Label.text = eventArgs.SelectedTab.Name;

        protected virtual void OnDestroy() => TabSelector.Selected -= OnTabSelected;
    }
}
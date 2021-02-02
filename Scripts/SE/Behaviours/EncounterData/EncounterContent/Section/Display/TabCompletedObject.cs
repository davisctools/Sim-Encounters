using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class TabCompletedObject : MonoBehaviour
    {
        protected ISelectedListener<UserTabSelectedEventArgs> TabSelector { get; set; }
        [Inject]
        public virtual void Inject(ISelectedListener<UserTabSelectedEventArgs> tabSelector)
            => TabSelector = tabSelector;

        protected virtual void Start()
        {
            TabSelector.Selected += OnSectionSelected;
            if (TabSelector.CurrentValue != null)
                OnSectionSelected(TabSelector, TabSelector.CurrentValue);
        }

        protected virtual UserTab CurrentTab { get; set; }
        protected virtual void OnSectionSelected(object sender, UserTabSelectedEventArgs eventArgs)
        {
            if (CurrentTab != null)
                CurrentTab.StatusChanged -= UpdateOn;

            CurrentTab = eventArgs.SelectedTab;
            CurrentTab.StatusChanged += UpdateOn;
            UpdateOn();
        }

        protected virtual void UpdateOn() => gameObject.SetActive(CurrentTab.IsRead());
    }
}
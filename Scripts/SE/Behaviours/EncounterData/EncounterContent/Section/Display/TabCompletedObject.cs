using System;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class TabCompletedObject : MonoBehaviour
    {
        [SerializeField] private bool updateWhenCurrentTabChanged;

        protected ILinearEncounterNavigator LinearEncounterNavigator { get; set; }
        protected ISelectedListener<UserTabSelectedEventArgs> TabSelector { get; set; }
        [Inject]
        public virtual void Inject(
            ILinearEncounterNavigator linearEncounterNavigator,
            ISelectedListener<UserTabSelectedEventArgs> tabSelector)
        {
            LinearEncounterNavigator = linearEncounterNavigator;
            TabSelector = tabSelector;

            TabSelector.Selected += OnTabSelected;
            if (TabSelector.CurrentValue != null)
                OnTabSelected(TabSelector, TabSelector.CurrentValue);

            if (updateWhenCurrentTabChanged)
                LinearEncounterNavigator.EncounterTabPositionChanged += EncounterTabPositionChanged;
        }

        private bool updateOnNextTabChange;
        protected virtual void EncounterTabPositionChanged(object sender, UserTabSelectedEventArgs e)
        {
            if (!updateOnNextTabChange || Tab == e.SelectedTab)
                return;
            updateOnNextTabChange = false;
            UpdateOn();
        }

        protected virtual UserTab Tab { get; set; }
        protected virtual void OnTabSelected(object sender, UserTabSelectedEventArgs eventArgs)
        {
            if (Tab != null)
                Tab.StatusChanged -= TabStatusChanged;

            Tab = eventArgs.SelectedTab;
            Tab.StatusChanged += TabStatusChanged;
            TabStatusChanged();
        }

        protected virtual void TabStatusChanged()
        {
            if (Tab == null || Tab.IsRead() == gameObject.activeSelf)
                return;

            if (updateWhenCurrentTabChanged && LinearEncounterNavigator.CurrentTab?.SelectedTab == Tab)
                updateOnNextTabChange = true;
            else
                UpdateOn();
        }

        protected virtual void UpdateOn() => gameObject.SetActive(Tab.IsRead());

        protected virtual void OnDestroy()
        {
            TabSelector.Selected -= OnTabSelected;
            if (updateWhenCurrentTabChanged)
                LinearEncounterNavigator.EncounterTabPositionChanged -= EncounterTabPositionChanged;
            if (Tab != null)
                Tab.StatusChanged -= TabStatusChanged;
        }
    }
}
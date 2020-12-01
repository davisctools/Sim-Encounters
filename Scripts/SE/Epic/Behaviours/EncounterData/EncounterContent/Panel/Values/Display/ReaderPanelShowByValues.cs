using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ReaderPanelShowByValues : MonoBehaviour
    {
        public List<string> HasAllValues { get => hasAllValues; set => hasAllValues = value; }
        [SerializeField] private List<string> hasAllValues;
        public List<string> HasAtLeastOneValue { get => hasAtLeastOneValue; set => hasAtLeastOneValue = value; }
        [SerializeField] private List<string> hasAtLeastOneValue;
        public List<GameObject> ShownObjects { get => shownObjects; set => shownObjects = value; }
        [SerializeField] private List<GameObject> shownObjects;
        public List<GameObject> HiddenObjects { get => hiddenObjects; set => hiddenObjects = value; }
        [SerializeField] private List<GameObject> hiddenObjects;
        protected ISelectedListener<PanelSelectedEventArgs> PanelSelectedListener { get; set; }
        [Inject]
        public virtual void Inject(ISelectedListener<PanelSelectedEventArgs> panelSelectedListener)
        {
            PanelSelectedListener = panelSelectedListener;
            PanelSelectedListener.Selected += OnPanelSelected;
            if (PanelSelectedListener.CurrentValue != null)
                OnPanelSelected(PanelSelectedListener, PanelSelectedListener.CurrentValue);
        }

        protected virtual void OnDestroy() => PanelSelectedListener.Selected -= OnPanelSelected;
        protected virtual void OnPanelSelected(object sender, PanelSelectedEventArgs eventArgs)
        {
            foreach (var valueKey in HasAllValues) {
                if (ToggleSelected(eventArgs.Panel.Values, valueKey))
                    continue;

                ControlObjects(false);
                return;
            }

            if (HasAtLeastOneValue.Count == 0) {
                ControlObjects(true);
                return;
            }

            foreach (var valueKey in HasAtLeastOneValue) {
                if (!ToggleSelected(eventArgs.Panel.Values, valueKey))
                    continue;

                ControlObjects(true);
                return;
            }

            ControlObjects(false);
        }

        protected virtual void ControlObjects(bool active)
        {
            foreach (var shownObject in ShownObjects)
                shownObject.SetActive(active);
            foreach (var hiddenObject in HiddenObjects)
                hiddenObject.SetActive(!active);
        }

        protected virtual bool ToggleSelected(IDictionary<string, string> values, string valueName)
        {
            if (!values.ContainsKey(valueName))
                return false;
            var value = values[valueName]?.Trim();

            return !value.Equals("0", StringComparison.InvariantCultureIgnoreCase)
                && !value.Equals("false", StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
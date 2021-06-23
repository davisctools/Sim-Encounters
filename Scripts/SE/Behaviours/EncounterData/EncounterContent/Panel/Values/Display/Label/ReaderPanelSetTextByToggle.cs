using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ReaderPanelSetTextByToggle : MonoBehaviour
    {
        public TextMeshProUGUI Label { get => label; set => label = value; }
        [SerializeField] private TextMeshProUGUI label;
        public List<PanelToggleText> TextOptions { get => textOptions; set => textOptions = value; }
        [SerializeField] private List<PanelToggleText> textOptions;
        public List<GameObject> ShownObjects { get => shownObjects; set => shownObjects = value; }
        [SerializeField] private List<GameObject> shownObjects;
        public List<GameObject> HiddenObjects { get => hiddenObjects; set => hiddenObjects = value; }
        [SerializeField] private List<GameObject> hiddenObjects;
        public string AlternateValueName { get => alternateValueName; set => alternateValueName = value; }
        [SerializeField] private string alternateValueName;
        public string AlternateIgnoreValue { get => alternateIgnoreValue; set => alternateIgnoreValue = value; }
        [SerializeField] private string alternateIgnoreValue;

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
            var values = eventArgs.Panel.Values;
            foreach (var textOption in TextOptions) {
                if (!ToggleSelected(values, textOption.ValueName))
                    continue;

                Label.text = textOption.Text;
                ControlObjects(true);
                return;
            }

            if (!values.ContainsKey(AlternateValueName)) {
                ControlObjects(false);
                return;
            }

            var value = eventArgs.Panel.Values[AlternateValueName].Trim();
            if (value.Equals(AlternateIgnoreValue, StringComparison.InvariantCultureIgnoreCase)) {
                ControlObjects(false);
                return;
            }

            Label.text = value;
            ControlObjects(true);
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

            return value.Equals("1", StringComparison.InvariantCultureIgnoreCase)
                || value.Equals("true", StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
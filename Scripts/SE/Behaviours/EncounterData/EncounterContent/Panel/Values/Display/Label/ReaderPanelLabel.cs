using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class ReaderPanelLabel : MonoBehaviour
    {
        [SerializeField] private string valueName = null;
        protected virtual string Name
        {
            get {
                if (!string.IsNullOrWhiteSpace(valueName))
                    return valueName;
                return name;
            }
        }

        public List<GameObject> ControlledObjects { get => controlledObjects; set => controlledObjects = value; }
        [SerializeField] private List<GameObject> controlledObjects;

        public string Prefix { get => prefix; set => prefix = value; }
        [Multiline] [SerializeField] private string prefix;
        public bool Trim { get => trim; set => trim = value; }
        [SerializeField] private bool trim;

        private TextMeshProUGUI label;
        protected TextMeshProUGUI Label
        {
            get {
                if (label == null)
                    label = GetComponent<TextMeshProUGUI>();
                return label;
            }
        }
        protected ISelectedListener<PanelSelectedEventArgs> PanelSelectedListener { get; set; }
        [Inject]
        public virtual void Inject(ISelectedListener<PanelSelectedEventArgs> panelSelectedListener)
        {
            PanelSelectedListener = panelSelectedListener;
            PanelSelectedListener.Selected += OnPanelSelected;
            if (PanelSelectedListener.CurrentValue != null)
                OnPanelSelected(PanelSelectedListener, PanelSelectedListener.CurrentValue);
        }

        protected Panel Panel { get; set; }
        protected virtual void OnDestroy() => PanelSelectedListener.Selected -= OnPanelSelected;
        protected virtual void OnPanelSelected(object sender, PanelSelectedEventArgs eventArgs)
        {
            if (Panel == eventArgs.Panel)
                return;

            Panel = eventArgs.Panel;

            if (!eventArgs.Panel.Values.ContainsKey(Name)) {
                HideControlledObjects();
                return;
            }

            var value = eventArgs.Panel.Values[Name];
            SetText(value);

            if (string.IsNullOrWhiteSpace(value))
                HideControlledObjects();
        }

        protected virtual void SetText(string value)
        {
            var text = "";
            if (Prefix != null)
                text += Prefix;
            if (value != null) {
                if (Trim)
                    value = value.Trim();

                text += value;
            }
            Label.text = text;
        }

        protected virtual void HideControlledObjects()
        {
            Label.text = "";
            foreach (var controlledObject in ControlledObjects) {
                if (controlledObject == null)
                    Debug.LogError(gameObject.name);
                else
                    controlledObject.SetActive(false);
            }
        }
    }
}
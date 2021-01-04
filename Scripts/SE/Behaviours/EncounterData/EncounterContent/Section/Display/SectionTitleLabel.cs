using TMPro;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class SectionTitleLabel : MonoBehaviour
    {
        protected TextMeshProUGUI Label => (label == null) ? label = GetComponent<TextMeshProUGUI>() : label;
        private TextMeshProUGUI label;

        protected ISelectedListener<SectionSelectedEventArgs> SectionSelector { get; set; }
        [Inject]
        public virtual void Inject(ISelectedListener<SectionSelectedEventArgs> sectionSelector)
        {
            SectionSelector = sectionSelector;
        }
        protected virtual void Start()
        {
            SectionSelector.Selected += OnSectionSelected;
            if (SectionSelector.CurrentValue != null)
                OnSectionSelected(SectionSelector, SectionSelector.CurrentValue);
        }
        protected virtual void OnSectionSelected(object sender, SectionSelectedEventArgs eventArgs)
            => Label.text = eventArgs.SelectedSection.Name;

        protected virtual void OnDestroy() => SectionSelector.Selected -= OnSectionSelected;
    }
}
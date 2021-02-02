using TMPro;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    [RequireComponent(typeof(TMP_Text))]
    public class SectionColorText : MonoBehaviour
    {
        protected TMP_Text Text => (text == null) ? text = GetComponent<TMP_Text>() : text;
        private TMP_Text text;

        protected ISelectedListener<SectionSelectedEventArgs> SectionSelector { get; set; }
        [Inject]
        public virtual void Inject(
            ISelectedListener<SectionSelectedEventArgs> sectionSelector)
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
            => Text.color = eventArgs.SelectedSection.Color;
    }
}
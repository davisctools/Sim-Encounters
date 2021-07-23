using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class SectionCompletedObject : MonoBehaviour
    {
        [SerializeField] private bool hideOnCompleted;

        protected ISelectedListener<UserSectionSelectedEventArgs> SectionSelector { get; set; }
        [Inject]
        public virtual void Inject(ISelectedListener<UserSectionSelectedEventArgs> sectionSelector)
            => SectionSelector = sectionSelector;

        protected virtual void Start()
        {
            SectionSelector.Selected += OnSectionSelected;
            if (SectionSelector.CurrentValue != null)
                OnSectionSelected(SectionSelector, SectionSelector.CurrentValue);
        }

        protected virtual UserSection CurrentSection { get; set; }
        protected virtual void OnSectionSelected(object sender, UserSectionSelectedEventArgs eventArgs)
        {
            if (CurrentSection != null)
                CurrentSection.StatusChanged -= UpdateOn;

            CurrentSection = eventArgs.SelectedSection;
            CurrentSection.StatusChanged += UpdateOn;
            UpdateOn();
        }

        protected virtual void UpdateOn() => gameObject.SetActive(CurrentSection.IsRead() ? !hideOnCompleted : hideOnCompleted);

        protected virtual void OnDestroy()
        {
            SectionSelector.Selected -= OnSectionSelected;
            if (CurrentSection != null)
                CurrentSection.StatusChanged -= UpdateOn;
        }
    }
}
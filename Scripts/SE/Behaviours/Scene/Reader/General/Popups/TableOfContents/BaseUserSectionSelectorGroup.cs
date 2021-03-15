using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class BaseUserSectionSelectorGroup<T> : MonoBehaviour
        where T : BaseSelectableUserSectionBehaviour
    {
        public RectTransform SectionsParent { get => sectionsParent; set => sectionsParent = value; }
        [SerializeField] private RectTransform sectionsParent;

        protected ISelectedListener<UserEncounterSelectedEventArgs> EncounterSelector { get; set; }
        protected PlaceholderFactory<T> SectionFactory { get; set; }
        [Inject]
        public virtual void Inject(
            ISelectedListener<UserEncounterSelectedEventArgs> encounterSelector,
            PlaceholderFactory<T> sectionFactory)
        {
            EncounterSelector = encounterSelector;
            SectionFactory = sectionFactory;
            EncounterSelector.Selected += OnEncounterSelected;
            if (EncounterSelector.CurrentValue != null)
                OnEncounterSelected(this, EncounterSelector.CurrentValue);
        }

        protected virtual void OnEncounterSelected(object sender, UserEncounterSelectedEventArgs e)
        {
            foreach (var sectionButton in SectionButtons.Values)
                Destroy(sectionButton.gameObject);
            SectionButtons.Clear();

            foreach (var section in e.Encounter.Sections.Values)
                DrawSection(section);
        }

        protected virtual Dictionary<UserSection, T> SectionButtons { get; } = new Dictionary<UserSection, T>();

        protected virtual void DrawSection(UserSection section)
        {
            var sectionObject = CreateSectionObject();
            sectionObject.Initialize(section);
            SectionButtons.Add(section, sectionObject);
        }

        protected virtual T CreateSectionObject()
        {
            var sectionObject = SectionFactory.Create();
            sectionObject.transform.SetParent(SectionsParent);
            sectionObject.transform.localScale = Vector3.one;
            return sectionObject;
        }
    }
}
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ReaderTableOfContents : MonoBehaviour
    {
        [SerializeField] private Transform sectionsParent;
        [SerializeField] private ToggleGroup toggleGroup;

        protected ISelectedListener<UserEncounterSelectedEventArgs> EncounterSelector { get; set; }
        protected BaseTableOfContentsSection.Factory SectionFactory { get; set; }
        [Inject]
        public virtual void Inject(
            ISelectedListener<UserEncounterSelectedEventArgs> encounterSelector, 
            BaseTableOfContentsSection.Factory sectionFactory)
        {
            EncounterSelector = encounterSelector;
            SectionFactory = sectionFactory;
        }

        protected virtual void Start()
        {
            EncounterSelector.Selected += EncounterSelected;
            if (EncounterSelector.CurrentValue != null)
                EncounterSelected(this, EncounterSelector.CurrentValue);
        }

        protected virtual void EncounterSelected(object sender, UserEncounterSelectedEventArgs e)
        {
            foreach (var section in e.Encounter.Sections.Values)
                DrawSection(section);
        }

        protected virtual void DrawSection(UserSection section)
        {
            var tableOfContentsTab = SectionFactory.Create();
            tableOfContentsTab.transform.SetParent(sectionsParent);
            tableOfContentsTab.transform.localScale = Vector3.one;
            tableOfContentsTab.Display(section);
            
            if (toggleGroup != null)
                tableOfContentsTab.SetToggleGroup(toggleGroup);
        }
    }
}
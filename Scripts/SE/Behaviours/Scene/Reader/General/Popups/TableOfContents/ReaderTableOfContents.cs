using UnityEngine;
using ClinicalTools.UI;
using UnityEngine.UI.Extensions;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ReaderTableOfContents : MonoBehaviour
    {
        [SerializeField] private Accordion accordion;
        [SerializeField] private Transform sectionsParent;

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
            SetAccordionToInstantTransition();
            foreach (var section in e.Encounter.Sections.Values)
                DrawSection(section);
            SetAccordionToTweenNextFrame();
        }

        protected virtual void DrawSection(UserSection section)
        {
            var tableOfContentsTab = SectionFactory.Create();
            tableOfContentsTab.transform.SetParent(sectionsParent);
            tableOfContentsTab.transform.localScale = Vector3.one;
            tableOfContentsTab.Initialize(section);
        }

        protected virtual void OnDisable() => SetAccordionToInstantTransition();
        protected virtual void OnEnable() => SetAccordionToTweenNextFrame();

        protected virtual void SetAccordionToTweenNextFrame() => NextFrame.Function(SetAccordionToTweenTransition);
        protected virtual void SetAccordionToInstantTransition() => accordion.transition = Accordion.Transition.Instant;
        protected virtual void SetAccordionToTweenTransition() => accordion.transition = Accordion.Transition.Tween;
    }
}
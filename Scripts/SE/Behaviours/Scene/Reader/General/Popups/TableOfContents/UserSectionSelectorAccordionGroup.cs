using UnityEngine;
using ClinicalTools.UI;
using UnityEngine.UI.Extensions;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class UserSectionSelectorAccordionGroup : MonoBehaviour
    {
        [SerializeField] private Accordion accordion;
        [SerializeField] private Transform sectionsParent;

        protected ISelectedListener<UserEncounterSelectedEventArgs> EncounterSelector { get; set; }
        protected BaseSelectableUserSectionBehaviour.Factory SectionFactory { get; set; }
        [Inject]
        public virtual void Inject(
            ISelectedListener<UserEncounterSelectedEventArgs> encounterSelector,
            BaseSelectableUserSectionBehaviour.Factory sectionFactory)
        {
            EncounterSelector = encounterSelector;
            SectionFactory = sectionFactory;
            EncounterSelector.Selected += EncounterSelected;
            if (EncounterSelector.CurrentValue != null)
                EncounterSelected(this, EncounterSelector.CurrentValue);
        }

        protected virtual void EncounterSelected(object sender, UserEncounterSelectedEventArgs e)
        {
            SetAccordionToInstantTransition();
            foreach (var section in e.Encounter.Sections.Values)
                DrawSection(section);
            if (gameObject.activeInHierarchy)
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
using UnityEngine;
using ClinicalTools.UI;
using UnityEngine.UI.Extensions;

namespace ClinicalTools.SimEncounters
{
    public class UserSectionSelectorAccordionGroup : BaseUserSectionSelectorGroup<BaseSelectableUserSectionBehaviour>
    {
        [SerializeField] private Accordion accordion;

        protected override void EncounterSelected(object sender, UserEncounterSelectedEventArgs e)
        {
            SetAccordionToInstantTransition();
            base.EncounterSelected(sender, e);
            if (gameObject.activeInHierarchy)
                SetAccordionToTweenNextFrame();
        }

        protected virtual void OnDisable() => SetAccordionToInstantTransition();
        protected virtual void OnEnable() => SetAccordionToTweenNextFrame();

        protected virtual void SetAccordionToTweenNextFrame() => NextFrame.Function(SetAccordionToTweenTransition);
        
        protected virtual void SetAccordionToInstantTransition() => accordion.transition = Accordion.Transition.Instant;
        protected virtual void SetAccordionToTweenTransition()
        {
            if (gameObject.activeInHierarchy)
                accordion.transition = Accordion.Transition.Tween;
        }
    }
}
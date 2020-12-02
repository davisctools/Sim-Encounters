using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class ReaderMobileSectionSelector : ReaderSectionSelector
    {
        public virtual Transform Line { get => line; set => line = value; }
        [SerializeField] private Transform line;
        protected override void OnEncounterSelected(object sender, UserEncounterSelectedEventArgs eventArgs)
        {
            base.OnEncounterSelected(sender, eventArgs);
            Line.SetAsLastSibling();
        }
    }
}
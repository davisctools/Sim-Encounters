using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    [RequireComponent(typeof(DifficultyUI))]
    public class EncounterDifficultyBehaviour : EncounterMetadataBehaviour
    {
        protected DifficultyUI DifficultyUI 
            => (difficultyUI == null) ? difficultyUI = GetComponent<DifficultyUI>() : difficultyUI;
        private DifficultyUI difficultyUI;

        protected override void OnMetadataSelected(object sender, EncounterMetadataSelectedEventArgs eventArgs)
            => DifficultyUI.Display(eventArgs.Metadata.Difficulty);
    }
}
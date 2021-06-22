using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    public class DifficultyFilterItem : EncounterFilterItem
    {
        public Toggle Toggle { get; set; }
        public Difficulty Difficulty { get; set; }
        public DifficultyFilterItem(Toggle toggle, string display, Difficulty difficulty) : base(display)
        {
            Toggle = toggle;
            Difficulty = difficulty;
        }
    }
}
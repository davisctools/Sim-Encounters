using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseViewEncounterSelector : BaseMenuEncounterSelectionManager
    {
        public abstract string ViewName { get; set; }
        public abstract Sprite ViewSprite { get; set; }
    }
}
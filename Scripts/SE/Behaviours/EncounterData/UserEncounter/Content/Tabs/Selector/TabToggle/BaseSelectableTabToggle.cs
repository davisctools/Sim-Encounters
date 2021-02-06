using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseSelectableTabToggle : BaseSelectableUserTabBehaviour
    {
        public abstract void SetToggleGroup(ToggleGroup group);
        public abstract void Select();
        public abstract void Deselect();
    }
}
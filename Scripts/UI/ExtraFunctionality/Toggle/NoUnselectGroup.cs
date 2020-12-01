using UnityEngine.UI;

namespace ClinicalTools.UI
{
    public class NoUnselectGroup : ToggleGroup
    {
        protected override void Awake()
        {
            allowSwitchOff = true;

            base.Awake();
        }

        protected virtual void Update()
        {
            if (allowSwitchOff && AnyTogglesOn())
                allowSwitchOff = false;
        }
    }
}

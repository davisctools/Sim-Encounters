using ClinicalTools.Collections;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseChildUserPanelsDrawer : MonoBehaviour
    {
        public abstract void Display(OrderedCollection<UserPanel> panels, bool active);
    }

}
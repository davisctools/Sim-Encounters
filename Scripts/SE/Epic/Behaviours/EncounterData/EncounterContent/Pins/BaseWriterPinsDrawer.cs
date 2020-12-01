using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseWriterPinsDrawer : MonoBehaviour
    {
        public abstract void Display(PinGroup pinData);

        public abstract PinGroup Serialize();
    }
}
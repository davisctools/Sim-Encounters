
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseSectionDrawer : MonoBehaviour
    {
        public abstract void Display(Encounter encounter, Section section);
    }
}
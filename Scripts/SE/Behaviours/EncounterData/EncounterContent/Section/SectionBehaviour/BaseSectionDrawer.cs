
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseSectionDrawer : MonoBehaviour
    {
        public abstract void Display(ContentEncounter encounter, Section section);
    }
}
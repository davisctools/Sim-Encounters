using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseSaveEncounterDisplay : MonoBehaviour
    {
        public abstract void Display(User user, Encounter encounter);
    }
}
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseReaderEncounterInfoPopup : MonoBehaviour
    {
        public abstract void ShowEncounterInfo(UserEncounter userEncounter);
    }
}
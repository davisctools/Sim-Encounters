using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseAddEncounterPopup : MonoBehaviour
    {
        public abstract void Display(MenuSceneInfo sceneInfo);
        public abstract void Display(MenuSceneInfo sceneInfo, MenuEncounter encounter);
    }
}

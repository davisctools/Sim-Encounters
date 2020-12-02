using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseMenuEncounterSelectionManager : MonoBehaviour
    {
        public abstract void DisplayForRead(MenuSceneInfo sceneInfo, IEnumerable<MenuEncounter> encounters);
        public abstract void DisplayForEdit(MenuSceneInfo sceneInfo, IEnumerable<MenuEncounter> encounters);
        public abstract void Initialize();
        public abstract void Hide();
    }
}
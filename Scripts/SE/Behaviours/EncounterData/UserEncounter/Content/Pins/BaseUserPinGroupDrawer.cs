using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseUserPinGroupDrawer : MonoBehaviour
    {
        public abstract void Display(UserPinGroup userPanel);
    }
}
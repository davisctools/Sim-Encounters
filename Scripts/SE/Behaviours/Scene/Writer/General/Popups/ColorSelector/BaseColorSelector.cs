using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseColorSelector : MonoBehaviour
    {
        public abstract WaitableTask<Color> SelectColor(Color color);
    }
}
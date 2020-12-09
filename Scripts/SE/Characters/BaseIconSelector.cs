using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseIconSelector : MonoBehaviour
    {
        public abstract WaitableTask<Icon> SelectIcon(Icon currentIcon);
    }
}
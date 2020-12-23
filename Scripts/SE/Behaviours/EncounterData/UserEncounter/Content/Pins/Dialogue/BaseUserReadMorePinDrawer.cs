using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseUserReadMorePinDrawer : MonoBehaviour
    {
        public abstract void Display(UserReadMorePin dialoguePin);

        public class Pool : SceneMonoMemoryPool<BaseUserReadMorePinDrawer>
        {
            public Pool(SignalBus signalBus) : base(signalBus) { }
        }
    }
}
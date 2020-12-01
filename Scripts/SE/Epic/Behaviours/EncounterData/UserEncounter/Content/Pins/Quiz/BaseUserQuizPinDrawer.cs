using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseUserQuizPinDrawer : MonoBehaviour
    {
        public abstract void Display(UserQuizPin quizPin);

        public class Pool : SceneMonoMemoryPool<BaseUserQuizPinDrawer>
        {
            public Pool(SignalBus signalBus) : base(signalBus) { }
        }
    }
}
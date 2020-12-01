using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseReaderTabToggle : MonoBehaviour
    {
        public abstract event Action Selected;
        public abstract void Display(UserTab tab);
        public abstract void SetToggleGroup(ToggleGroup group);
        public abstract void Select();

        public class Pool : SceneMonoMemoryPool<BaseReaderTabToggle>
        {
            public Pool(SignalBus signalBus) : base(signalBus) { }
        }
    }
}
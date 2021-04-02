using System;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseEncounterImageOption : MonoBehaviour
    {
        public abstract event Action<EncounterImage> ImageSelected;
        public abstract event Action<EncounterImage> ImageDeselected;

        public abstract void Initialize(EncounterImage image);

        public class Pool : SceneMonoMemoryPool<BaseEncounterImageOption>
        {
            public Pool(SignalBus signalBus) : base(signalBus) { }
        }
    }
}
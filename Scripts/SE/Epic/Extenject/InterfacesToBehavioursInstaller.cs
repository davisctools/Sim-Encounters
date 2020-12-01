using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class InterfacesToBehavioursInstaller : MonoInstaller
    {
        public List<MonoBehaviour> Behaviours { get => behaviours; }
        [SerializeField] private List<MonoBehaviour> behaviours = new List<MonoBehaviour>();

        public override void InstallBindings()
        {
            foreach (var behaviour in Behaviours)
                Container.BindInterfacesTo(behaviour.GetType()).FromInstance(behaviour);
        }
    }
}
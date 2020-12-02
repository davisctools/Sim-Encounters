using System;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{

    public delegate bool Filter<in T>(T x);
    public abstract class EncounterFilterBehaviour : MonoBehaviour
    {
        public abstract Filter<MenuEncounter> EncounterFilter { get; }
        
        public abstract event Action<Filter<MenuEncounter>> FilterChanged;

        public abstract void Clear();
    }
}
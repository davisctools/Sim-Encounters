using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ShowOnEncounterCompleted : MonoBehaviour
    {
        protected SignalBus SignalBus { get; set; }
        [Inject]
        public virtual void Inject(SignalBus signalBus)
        {
            SignalBus = signalBus;
            SignalBus.Subscribe<EncounterCompletedSignal>(OnEncounterCompleted);
        }

        protected virtual void OnEncounterCompleted() => gameObject.SetActive(true);
    }
}
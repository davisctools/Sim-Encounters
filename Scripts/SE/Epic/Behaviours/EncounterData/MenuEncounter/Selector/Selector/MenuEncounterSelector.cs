using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public abstract class MenuEncounterSelector : MonoBehaviour,
        ISelector<MenuEncounterSelectedEventArgs>,
        ISelectedListener<EncounterMetadataSelectedEventArgs>
    {
        public MenuEncounterSelectedEventArgs CurrentValue { get; protected set; }
        protected EncounterMetadataSelectedEventArgs CurrentMetadataValue { get; set; }
        EncounterMetadataSelectedEventArgs ISelectedListener<EncounterMetadataSelectedEventArgs>.CurrentValue => CurrentMetadataValue;

        public event SelectedHandler<MenuEncounterSelectedEventArgs> Selected;
        public event SelectedHandler<EncounterMetadataSelectedEventArgs> MetadataSelected;

        event SelectedHandler<EncounterMetadataSelectedEventArgs> ISelectedListener<EncounterMetadataSelectedEventArgs>.Selected {
            add => MetadataSelected += value;
            remove => MetadataSelected -= value;
        }

        public virtual void Select(object sender, MenuEncounterSelectedEventArgs eventArgs)
        {
            CurrentValue = eventArgs;
            Selected?.Invoke(sender, eventArgs);

            CurrentMetadataValue = new EncounterMetadataSelectedEventArgs(eventArgs.Encounter.GetLatestMetadata());
            MetadataSelected?.Invoke(sender, CurrentMetadataValue);
        }

        public class Pool : SceneMonoMemoryPool<MenuEncounterSelector>
        {
            public Pool(SignalBus signalBus) : base(signalBus) { }
        }
    }
}
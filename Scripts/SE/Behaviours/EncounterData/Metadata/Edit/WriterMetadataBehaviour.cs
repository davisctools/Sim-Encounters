using Zenject;

namespace ClinicalTools.SimEncounters
{
    public abstract class WriterMetadataBehaviour : EncounterMetadataBehaviour
    {
        protected SignalBus SignalBus { get; set; }
        [Inject] public virtual void Inject(SignalBus signalBus) => SignalBus = signalBus;

        protected override void Start()
        {
            base.Start();
            SignalBus.Subscribe<SerializeEncounterSignal>(OnSerializeEncounterSignal);
        }

        protected virtual void OnSerializeEncounterSignal() => Serialize(MetadataSelector.CurrentValue.Metadata);
        protected abstract void Serialize(OldEncounterMetadata metadata);
    }
}
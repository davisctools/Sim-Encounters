using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public abstract class EncounterMetadataBehaviour : MonoBehaviour
    {
        protected ISelectedListener<EncounterMetadataSelectedEventArgs> MetadataSelector { get; set; }
        [Inject] public virtual void Inject(ISelectedListener<EncounterMetadataSelectedEventArgs> metadataSelector) => MetadataSelector = metadataSelector;
        protected virtual void Start()
        {
            MetadataSelector.Selected += OnMetadataSelected;
            if (MetadataSelector.CurrentValue != null)
                OnMetadataSelected(MetadataSelector, MetadataSelector.CurrentValue);
        }
        protected abstract void OnMetadataSelected(object sender, EncounterMetadataSelectedEventArgs eventArgs);
        protected virtual void OnDestroy() => MetadataSelector.Selected -= OnMetadataSelected;
    }
}
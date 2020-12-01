using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    public class WriterEncounterPrivateToggle : WriterMetadataToggle
    {
        protected override void OnMetadataSelected(object sender, EncounterMetadataSelectedEventArgs eventArgs)
            => Toggle.isOn = !eventArgs.Metadata.IsPublic;

        protected override void Serialize(EncounterMetadata metadata)
            => metadata.IsPublic = !Toggle.isOn;
    }
}
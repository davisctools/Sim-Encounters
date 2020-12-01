using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    public class WriterEncounterTemplateToggle : WriterMetadataToggle
    {
        protected override void OnMetadataSelected(object sender, EncounterMetadataSelectedEventArgs eventArgs)
            => Toggle.isOn = eventArgs.Metadata.IsTemplate;

        protected override void Serialize(EncounterMetadata metadata)
            => metadata.IsTemplate = Toggle.isOn;
    }
}
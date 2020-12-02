﻿namespace ClinicalTools.SimEncounters
{
    public class WriterEncounterTitleInputField : WriterMetadataInputField
    {
        protected override void OnMetadataSelected(object sender, EncounterMetadataSelectedEventArgs eventArgs)
            => InputField.text = eventArgs.Metadata.Title;
        protected override void Serialize(EncounterMetadata metadata) => metadata.Title = InputField.text;
    }
}
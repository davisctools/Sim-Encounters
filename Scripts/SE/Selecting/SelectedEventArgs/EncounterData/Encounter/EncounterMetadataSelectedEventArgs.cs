using System;

namespace ClinicalTools.SimEncounters
{
    public class EncounterMetadataSelectedEventArgs : EventArgs
    {
        public EncounterMetadata Metadata { get; }
        public EncounterMetadataSelectedEventArgs(EncounterMetadata metadata) => Metadata = metadata;
    }
}
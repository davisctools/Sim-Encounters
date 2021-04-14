using System;

namespace ClinicalTools.SimEncounters
{
    public class EncounterMetadataSelectedEventArgs : EventArgs
    {
        public OldEncounterMetadata Metadata { get; }
        public EncounterMetadataSelectedEventArgs(OldEncounterMetadata metadata) => Metadata = metadata;
    }
}
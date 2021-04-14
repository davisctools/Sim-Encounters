using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public class NewMenuEncounter
    {
        public EncounterMetadata PrimaryMetadata { get; }

        public EncounterFileSave LocalSave { get; set; }
        public EncounterFileSave LocalAutoSave { get; set; }

        public EncounterSave LatestSave { get; set; }
        public EncounterSave PublicSave { get; set; }
        public EncounterSave AutoSave { get; set; }

        public bool Public { get; set; }

        // Keyed by Mutable Metadata Save ID
        public Dictionary<int, EncounterMutableMetadata> MutableMetadata { get; } = new Dictionary<int, EncounterMutableMetadata>();

        public List<EncounterSave> Saves { get; } = new List<EncounterSave>();

        public NewMenuEncounter(EncounterImmutableMetadata immutableMetadata)
            => PrimaryMetadata = new EncounterMetadata(immutableMetadata);
    }
}
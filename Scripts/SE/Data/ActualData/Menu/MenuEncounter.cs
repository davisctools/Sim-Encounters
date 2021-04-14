using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public enum SaveType
    {
        Default, Autosave, Demo, Local, Server
    }

    public class MenuEncounter
    {
        public EncounterEditLock Lock { get; set; }

        public Dictionary<SaveType, OldEncounterMetadata> Metadata { get; }
        public EncounterBasicStatus Status { get; set; }

        public MenuEncounter(Dictionary<SaveType, OldEncounterMetadata> metadata, EncounterBasicStatus status)
        {
            Metadata = metadata;
            Status = status;
        }

        public virtual KeyValuePair<SaveType, OldEncounterMetadata> GetLatestTypedMetada()
        {
            var latest = new KeyValuePair<SaveType, OldEncounterMetadata>();
            foreach (var metadata in Metadata) {
                if (metadata.Key == SaveType.Local)
                    return metadata;

                if (latest.Value == null || latest.Value.DateModified < metadata.Value.DateModified)
                    latest = metadata;
            }

            return latest;
        }
        public virtual SaveType GetLatestType() => GetLatestTypedMetada().Key;
        public virtual OldEncounterMetadata GetLatestMetadata() => GetLatestTypedMetada().Value;
    }
}
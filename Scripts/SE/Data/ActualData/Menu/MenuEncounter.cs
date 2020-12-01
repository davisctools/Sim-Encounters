using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public enum SaveType
    {
        Default, Autosave, Demo, Local, Server
    }
    public class MenuEncounter
    {
        public Dictionary<SaveType, EncounterMetadata> Metadata { get; }
        public EncounterBasicStatus Status { get; set; }

        public MenuEncounter(Dictionary<SaveType, EncounterMetadata> metadata, EncounterBasicStatus status)
        {
            Metadata = metadata;
            Status = status;
        }

        public virtual KeyValuePair<SaveType, EncounterMetadata> GetLatestTypedMetada()
        {
            var latest = new KeyValuePair<SaveType, EncounterMetadata>();
            foreach (var metadata in Metadata)
            {
                if (metadata.Key == SaveType.Local)
                    return metadata;

                if (latest.Value == null || latest.Value.DateModified < metadata.Value.DateModified)
                    latest = metadata;
            }

            return latest;
        }
        public virtual SaveType GetLatestType() => GetLatestTypedMetada().Key;
        public virtual EncounterMetadata GetLatestMetadata() => GetLatestTypedMetada().Value;
        public virtual void IsMetadataNewer() {

        }
    }
}
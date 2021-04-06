using SimpleJSON;

namespace ClinicalTools.SimEncounters
{
    public class EncounterEditLockDeserializer : IStringDeserializer<EncounterEditLock>, IJsonDeserializer<EncounterEditLock>
    {
        public EncounterEditLock Deserialize(string text)
        {
            var node = JSON.Parse(text);
            return node != null ? Deserialize(node) : null;
        }

        public virtual EncounterEditLock Deserialize(JSONNode node)
            => new EncounterEditLock {
                RecordNumber = node["id"],
                EditorName = node["editor"],
                StartEditTime = node["start"]
            };
    }
}
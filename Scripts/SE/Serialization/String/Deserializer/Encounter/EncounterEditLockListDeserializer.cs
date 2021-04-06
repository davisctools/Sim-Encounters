using SimpleJSON;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public class EncounterEditLockListDeserializer : IStringDeserializer<List<EncounterEditLock>>, IJsonDeserializer<List<EncounterEditLock>>
    {
        protected IJsonDeserializer<EncounterEditLock> LockDeserializer { get; }
        public EncounterEditLockListDeserializer(IJsonDeserializer<EncounterEditLock> lockDeserializer) => LockDeserializer = lockDeserializer;

        public virtual List<EncounterEditLock> Deserialize(string text)
        {
            var node = JSON.Parse(text);
            return node != null ? Deserialize(node) : new List<EncounterEditLock>();
        }

        public virtual List<EncounterEditLock> Deserialize(JSONNode node)
        {
            var locksNode = node["locks"];
            if (locksNode == null)
                locksNode = node;
            
            var locks = new List<EncounterEditLock>();
            foreach (var child in locksNode.Children) {
                var encounterLock = LockDeserializer.Deserialize(child);
                if (encounterLock != null)
                    locks.Add(encounterLock);
            }
            
            return locks;
        }
    }
}
using ClinicalTools.SimEncounters;
using SimpleJSON;

namespace ClinicalTools.Lift
{
    public class EncounterImageJsonDeserializer : IJsonDeserializer<EncounterImage>, IStringDeserializer<EncounterImage>
    {
        public virtual EncounterImage Deserialize(string text)
        {
            var node = JSON.Parse(text);
            return node != null ? Deserialize(node) : null;
        }

        public virtual EncounterImage Deserialize(JSONNode node)
            => new EncounterImage() {
                DateModified = node["date"],
                Id = node["id"],
                Key = node["key"],
                FileName = node["file"]
            };
    }
}
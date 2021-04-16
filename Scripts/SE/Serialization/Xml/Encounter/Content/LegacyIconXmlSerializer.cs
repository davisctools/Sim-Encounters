using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class LegacyIconXmlSerializer : IconXmlSerializer
    {
        public LegacyIconXmlSerializer(IObjectSerializer<Color> colorSerializer) : base(colorSerializer) { }

        protected virtual XmlNodeInfo LegacyReferenceInfo { get; } = XmlNodeInfo.RootValue;
        protected override string GetReference(IDataDeserializer deserializer)
        {
            var reference = base.GetReference(deserializer);
            if (!string.IsNullOrEmpty(reference))
                return reference;
            return deserializer.GetString(LegacyReferenceInfo);
        }
    }
}
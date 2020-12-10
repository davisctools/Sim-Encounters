namespace ClinicalTools.SimEncounters
{
    public class LegacyIconXmlSerializer : IconXmlSerializer
    {
        protected virtual XmlNodeInfo LegacyReferenceInfo { get; } = XmlNodeInfo.RootValue;
        protected override string GetReference(XmlDeserializer deserializer)
        {
            var reference = base.GetReference(deserializer);
            if (!string.IsNullOrEmpty(reference))
                return reference;
            return deserializer.GetString(LegacyReferenceInfo);
        }
    }
}
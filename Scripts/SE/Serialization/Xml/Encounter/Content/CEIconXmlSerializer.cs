namespace ClinicalTools.SimEncounters
{
    public class CEIconXmlSerializer : IconXmlSerializer {

        protected virtual XmlNodeInfo LegacyReferenceInfo { get; } = XmlNodeInfo.RootValue;

        public override Icon Deserialize(XmlDeserializer deserializer)
        {
            var type = GetType(deserializer);
            var reference = GetReference(deserializer);
            var color = GetColor(deserializer);

            return new Icon(type, reference, color);
        }

        protected override string GetReference(XmlDeserializer deserializer)
        {
            var reference = base.GetReference(deserializer);
            if (!string.IsNullOrEmpty(reference))
                return reference;
            return deserializer.GetString(LegacyReferenceInfo);
        }

    }
}

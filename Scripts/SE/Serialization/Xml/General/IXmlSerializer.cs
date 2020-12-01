namespace ClinicalTools.SimEncounters
{
    public interface IXmlSerializer<T>
    {
        bool ShouldSerialize(T value);
        void Serialize(XmlSerializer serializer, T value);
        T Deserialize(XmlDeserializer deserializer);
    }
}
namespace ClinicalTools.SimEncounters
{
    public interface IObjectSerializer<T>
    {
        bool ShouldSerialize(T value);
        void Serialize(IDataSerializer serializer, T value);
        T Deserialize(IDataDeserializer deserializer);
    }
}
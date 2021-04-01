using SimpleJSON;

namespace ClinicalTools.SimEncounters
{
    public interface IJsonDeserializer<T>
    {
        T Deserialize(JSONNode node);
    }
}
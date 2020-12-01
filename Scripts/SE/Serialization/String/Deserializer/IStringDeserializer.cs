namespace ClinicalTools.SimEncounters
{
    public interface IStringDeserializer<T>
    {
        T Deserialize(string text);
    }
}
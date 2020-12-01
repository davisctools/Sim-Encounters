namespace ClinicalTools.SimEncounters
{
    public interface ICharEnumeratorDeserializer<T>
    {
        T Deserialize(CharEnumerator enumerator);
    }
}
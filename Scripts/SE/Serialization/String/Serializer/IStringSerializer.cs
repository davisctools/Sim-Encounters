namespace ClinicalTools.SimEncounters
{
    public interface IStringSerializer<T>
    {
        string Serialize(T value);
    }
}

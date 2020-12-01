namespace ClinicalTools.Collections
{
    public interface IKeyGenerator
    {
        bool Contains(string key);
        void AddKey(string key);
        string Generate();
        string Generate(string seed);
        bool IsValidKey(string key);
    }
}
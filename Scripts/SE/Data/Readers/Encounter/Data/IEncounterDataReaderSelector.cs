namespace ClinicalTools.SimEncounters
{
    public interface IEncounterDataReaderSelector
    {
        IEncounterDataReader GetEncounterDataReader(SaveType saveType);
    }
}
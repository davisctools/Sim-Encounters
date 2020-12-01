using System.Collections.Generic;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class EncounterDataReaderSelector : IEncounterDataReaderSelector
    {
        protected virtual Dictionary<SaveType, IEncounterDataReader> EncounterReaders { get; } = new Dictionary<SaveType, IEncounterDataReader>();
        public EncounterDataReaderSelector(
            [Inject(Id = SaveType.Default)] IEncounterDataReader defaultReader,
            [Inject(Id = SaveType.Autosave)] IEncounterDataReader autosaveReader,
            [Inject(Id = SaveType.Demo)] IEncounterDataReader demoReader,
            [Inject(Id = SaveType.Local)] IEncounterDataReader localReader,
            [Inject(Id = SaveType.Server)] IEncounterDataReader serverReader)
        {
            EncounterReaders.Add(SaveType.Default, defaultReader);
            EncounterReaders.Add(SaveType.Autosave, autosaveReader);
            EncounterReaders.Add(SaveType.Demo, demoReader);
            EncounterReaders.Add(SaveType.Local, localReader);
            EncounterReaders.Add(SaveType.Server, serverReader);
        }

        public IEncounterDataReader GetEncounterDataReader(SaveType saveType) => EncounterReaders[saveType];
    }
}
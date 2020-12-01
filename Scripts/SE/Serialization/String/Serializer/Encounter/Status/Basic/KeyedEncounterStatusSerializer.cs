using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public class KeyedEncounterStatusSerializer : IStringSerializer<KeyValuePair<int, EncounterBasicStatus>>
    {
        private const string CaseInfoDivider = "|";

        public string Serialize(KeyValuePair<int, EncounterBasicStatus> value)
            => $"{value.Key}{CaseInfoDivider}" +
               $"{value.Value.Timestamp}{CaseInfoDivider}" +
               $"{(value.Value.Completed ? "1" : "0")}{CaseInfoDivider}" +
               $"{value.Value.Rating}";
    }
}

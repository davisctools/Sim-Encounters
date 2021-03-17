using System;

namespace ClinicalTools.SimEncounters
{
    public class EncounterEditLockDeserializer : IStringDeserializer<EncounterEditLock>
    {
        // Ideally I'd use JSON objects, and I've set the PHP to allow them.
        // Unfortunately, it would drastically increase the size of the menu information the server gives
        // It would also cause more work to support it, so for now, I'll stick with more simplistic storage methods
        private const char CaseInfoDivider = '|';
        private const int EncounterParts = 3;
        private const int RecordNumberIndex = 0;
        private const int EditorIndex = 1;
        private const int StartTimeIndex = 2;

        public EncounterEditLock Deserialize(string text)
        {
            try {
                var parsedItem = text.Split(CaseInfoDivider);
                if (parsedItem == null || parsedItem.Length < EncounterParts)
                    return null;

                var encounterLock = new EncounterEditLock() {
                    RecordNumber = int.Parse(parsedItem[RecordNumberIndex]),
                    EditorName = parsedItem[EditorIndex],
                    StartEditTime = long.Parse(parsedItem[StartTimeIndex])
                };

                return encounterLock;
            } catch (Exception) {
                return null;
            }
        }
    }
}
using System;

namespace ClinicalTools.SimEncounters
{
    public class EncounterAlreadyLockedException : Exception
    {
        public EncounterEditLock Lock { get; }
        public EncounterAlreadyLockedException(EncounterEditLock editLock) : base("Encounter already locked.")
            => Lock = editLock;
    }
}

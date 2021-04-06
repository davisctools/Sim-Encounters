using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public interface IEncounterLocksReader
    {
        WaitableTask<Dictionary<int, EncounterEditLock>> GetEncounterLocks(User user);
    }
}
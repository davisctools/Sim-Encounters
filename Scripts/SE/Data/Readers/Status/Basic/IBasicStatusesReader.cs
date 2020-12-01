using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public interface IBasicStatusesReader
    {
        WaitableTask<Dictionary<int, EncounterBasicStatus>> GetBasicStatuses(User user);
    }
}
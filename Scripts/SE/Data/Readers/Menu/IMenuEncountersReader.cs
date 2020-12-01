using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public interface IMenuEncountersReader
    {
        WaitableTask<List<MenuEncounter>> GetMenuEncounters(User user);
    }
}
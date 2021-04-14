using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public interface IMetadataGroupsReader
    {
        WaitableTask<Dictionary<int, Dictionary<SaveType, OldEncounterMetadata>>> GetMetadataGroups(User user);
    }
}
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public interface IMetadatasReader
    {
        WaitableTask<List<OldEncounterMetadata>> GetMetadatas(User user);
    }
}

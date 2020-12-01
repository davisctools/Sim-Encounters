using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public interface IMetadatasReader
    {
        WaitableTask<List<EncounterMetadata>> GetMetadatas(User user);
    }
}

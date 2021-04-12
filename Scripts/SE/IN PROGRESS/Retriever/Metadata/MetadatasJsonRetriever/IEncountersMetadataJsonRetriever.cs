using SimpleJSON;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public interface IEncountersMetadataJsonRetriever
    {
        WaitableTask<IEnumerable<JSONNode>> GetMetadataJsonNodes(User user);
    }
}

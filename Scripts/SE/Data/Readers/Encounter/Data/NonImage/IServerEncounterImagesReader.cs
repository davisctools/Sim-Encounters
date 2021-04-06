using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public interface IServerEncounterImagesReader
    {
        WaitableTask<List<EncounterImage>> GetImages(User user, EncounterMetadata metadata);
    }
}
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public interface IEncounterImagesReader
    {
        WaitableTask<List<EncounterImage>> GetImages(User user, EncounterMetadata metadata);
    }
}
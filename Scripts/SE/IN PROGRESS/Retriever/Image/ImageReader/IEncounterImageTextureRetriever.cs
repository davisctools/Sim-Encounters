using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public interface IEncounterImageTextureRetriever
    {
        WaitableTask<Texture2D> GetTexture(User user, OldEncounterMetadata metadata, EncounterImage image);
    }
}

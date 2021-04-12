using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public interface IEncounterImageTextureRetriever
    {
        WaitableTask<Texture2D> GetTexture(User user, EncounterMetadata metadata, EncounterImage image);
    }
}

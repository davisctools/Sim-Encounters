using UnityEngine;
using UnityEngine.Networking;

namespace ClinicalTools.SimEncounters
{
    public interface IServerTextureReader
    {
        WaitableTask<Texture2D> Begin(UnityWebRequest webRequest);
    }
}
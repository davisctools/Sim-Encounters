using UnityEngine.Networking;

namespace ClinicalTools.SimEncounters
{
    public interface IServerReader
    {
        WaitableTask<string> Begin(UnityWebRequest webRequest);
    }
}
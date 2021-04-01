using UnityEngine.Networking;

namespace ClinicalTools.SimEncounters
{
    public interface IServerStringReader
    {
        WaitableTask<string> Begin(UnityWebRequest webRequest);
    }
}
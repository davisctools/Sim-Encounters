using UnityEngine;
using UnityEngine.Networking;

namespace ClinicalTools.SimEncounters
{
    public class ServerTextureReader : IServerTextureReader
    {
        public WaitableTask<Texture2D> Begin(UnityWebRequest webRequest)
        {
            var result = new WaitableTask<Texture2D>();
            var requestOperation = webRequest.SendWebRequest();
            requestOperation.completed += (asyncOperation) => ProcessWebrequest(webRequest, result);
            return result;
        }

        protected void ProcessWebrequest(UnityWebRequest webRequest, WaitableTask<Texture2D> result)
        {
            try {
                var serverResult = GetResults(webRequest);
                webRequest.Dispose();
                result.SetResult(serverResult);
            } catch (WebException webException) {
                webRequest.Dispose();
                result.SetError(webException);
            }
        }

        protected Texture2D GetResults(UnityWebRequest webRequest)
        {
            if (!webRequest.isDone)
                throw new WebException(ServerOutcome.WebRequestNotDone, webRequest.error);
            else if (webRequest.isNetworkError)
                throw new WebException(ServerOutcome.NetworkError, webRequest.error);
            else if (webRequest.isHttpError)
                throw new HttpErrorException(webRequest.error, webRequest.responseCode, webRequest.downloadHandler?.text);
            else if (!webRequest.downloadHandler.isDone)
                throw new WebException(ServerOutcome.DownloadNotDone, webRequest.error);

            return DownloadHandlerTexture.GetContent(webRequest);
        }
    }
}
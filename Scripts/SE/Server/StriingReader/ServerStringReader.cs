using UnityEngine.Networking;

namespace ClinicalTools.SimEncounters
{
    public class ServerStringReader : IServerStringReader
    {
        /**
         * Downloads all available and applicable menu files to display on the main manu.
         * Returns them as a MenuCase item
         */
        public WaitableTask<string> Begin(UnityWebRequest webRequest)
        {
            var result = new WaitableTask<string>();
            var requestOperation = webRequest.SendWebRequest();
            requestOperation.completed += (asyncOperation) => ProcessWebrequest(webRequest, result);
            return result;
        }

        protected void ProcessWebrequest(UnityWebRequest webRequest, WaitableTask<string> result)
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

        protected string GetResults(UnityWebRequest webRequest)
        {
            if (!webRequest.isDone)
                throw new WebException(ServerOutcome.WebRequestNotDone, webRequest.error);
            else if (webRequest.isNetworkError)
                throw new WebException(ServerOutcome.NetworkError, webRequest.error);
            else if (webRequest.isHttpError)
                throw new HttpErrorException(webRequest.error, webRequest.responseCode, webRequest.downloadHandler?.text);
            else if (!webRequest.downloadHandler.isDone)
                throw new WebException(ServerOutcome.DownloadNotDone, webRequest.error);

            return webRequest.downloadHandler.text
                        .Replace("â€™", "'")
                        .Replace("â€‹", "");
        }
    }
}
﻿using UnityEngine.Networking;

namespace ClinicalTools.SimEncounters
{
    public class ServerReader : IServerReader
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
                throw new WebException(ServerOutcome.DownloadNotDone, webRequest.error);
            else if (webRequest.isNetworkError)
                throw new WebException(ServerOutcome.NetworkError, webRequest.error);
            else if (webRequest.isHttpError)
                throw new WebException(ServerOutcome.HttpError, webRequest.error);
            else if (!webRequest.downloadHandler.isDone)
                throw new WebException(ServerOutcome.DownloadNotDone, webRequest.error);

            return webRequest.downloadHandler.text
                        .Replace("â€™", "'")
                        .Replace("â€‹", "");
        }
    }
}
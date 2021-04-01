using System;
using UnityEngine;
using UnityEngine.Networking;

namespace ClinicalTools.SimEncounters
{
    public class ServerEncounterRemover : IEncounterRemover
    {
        protected IServerStringReader ServerReader { get; }
        protected IUrlBuilder UrlBuilder { get; }
        public ServerEncounterRemover(IServerStringReader serverReader, IUrlBuilder urlBuilder)
        {
            ServerReader = serverReader;
            UrlBuilder = urlBuilder;
        }

        protected virtual string PhpFile { get; } = "DeleteEncounter.php";
        public WaitableTask Delete(User user, EncounterMetadata metadata)
        {
            if (user.IsGuest)
                return WaitableTask.CompletedTask;

            var url = UrlBuilder.BuildUrl(PhpFile);
            var form = CreateForm(user, metadata);

            var webRequest = UnityWebRequest.Post(url, form);
            var serverResults = ServerReader.Begin(webRequest);

            var result = new WaitableTask();
            serverResults.AddOnCompletedListener((serverResult) => ProcessResults(result, serverResult, metadata));
            return result;
        }


        private const string AccountIdVariable = "accountId";
        private const string RecordNumberVariable = "recordNumber";
        private const string ModeVariable = "mode";
        private const string DeleteModeValue = "delete";
        protected virtual WWWForm CreateForm(User user, EncounterMetadata metadata)
        {
            var form = new WWWForm();

            form.AddField(ModeVariable, DeleteModeValue);
            form.AddField(AccountIdVariable, user.AccountId);
            form.AddField(RecordNumberVariable, metadata.RecordNumber);

            return form;
        }

        private void ProcessResults(WaitableTask actionResult, TaskResult<string> serverResult, EncounterMetadata metadata)
        {
            if (serverResult.IsError()) {
                actionResult.SetError(serverResult.Exception);
                return;
            }

            Debug.Log("Returned text from PHP: \n" + serverResult.Value);
            if (string.IsNullOrWhiteSpace(serverResult.Value))
                actionResult.SetError(new Exception("No text returned from the server."));
            else if (!int.TryParse(serverResult.Value, out int recordNumber))
                actionResult.SetError(new Exception(serverResult.Value));
            else if (recordNumber != metadata.RecordNumber)
                actionResult.SetError(new Exception("Did not get the correct metadata from the server."));
            else
                actionResult.SetCompleted();
        }
    }
}

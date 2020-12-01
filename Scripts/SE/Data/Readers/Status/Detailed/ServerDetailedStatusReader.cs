
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace ClinicalTools.SimEncounters
{
    public class ServerDetailedStatusReader : IDetailedStatusReader
    {
        private readonly IUrlBuilder urlBuilder;
        private readonly IServerReader serverReader;
        private readonly IStringDeserializer<EncounterContentStatus> parser;
        public ServerDetailedStatusReader(IUrlBuilder urlBuilder, IServerReader serverReader, IStringDeserializer<EncounterContentStatus> parser)
        {
            this.urlBuilder = urlBuilder;
            this.serverReader = serverReader;
            this.parser = parser;
        }

        public WaitableTask<EncounterStatus> GetDetailedStatus(User user,
            EncounterMetadata metadata, EncounterBasicStatus basicStatus)
        {
            if (user.IsGuest)
                return new WaitableTask<EncounterStatus>(
                    new EncounterStatus(new EncounterBasicStatus(), new EncounterContentStatus()));

            var webRequest = GetWebRequest(user, metadata);
            var serverOutput = serverReader.Begin(webRequest);
            var detailedStatus = new WaitableTask<EncounterStatus>();
            serverOutput.AddOnCompletedListener((result) => ProcessResults(detailedStatus, result, basicStatus));

            return detailedStatus;
        }

        private const string menuPhp = "Track.php";
        private const string actionVariable = "ACTION";
        private const string downloadAction = "download";
        private const string usernameVariable = "username";
        private const string recordNumberVariable = "recordNumber";
        protected UnityWebRequest GetWebRequest(User user, EncounterMetadata metadata)
        {
            var url = urlBuilder.BuildUrl(menuPhp);
            var form = new WWWForm();

            form.AddField(actionVariable, downloadAction);
            form.AddField(usernameVariable, user.Username);
            form.AddField(recordNumberVariable, metadata.RecordNumber);

            return UnityWebRequest.Post(url, form);
        }

        private void ProcessResults(WaitableTask<EncounterStatus> result,
            TaskResult<string> serverOutput, EncounterBasicStatus basicStatus)
        {
            if (serverOutput == null || serverOutput.IsError())
            {
                result.SetError(new Exception(serverOutput?.Value));
                return;
            }

            var contentStatus = parser.Deserialize(serverOutput.Value);
            var status = new EncounterStatus(basicStatus, contentStatus);
            result.SetResult(status);
        }
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace ClinicalTools.SimEncounters
{
    public class ServerBasicStatusesReader : IBasicStatusesReader
    {
        private readonly IUrlBuilder urlBuilder;
        private readonly IServerReader serverReader;
        private readonly IStringDeserializer<Dictionary<int, EncounterBasicStatus>> parser;
        public ServerBasicStatusesReader(IUrlBuilder urlBuilder, IServerReader serverReader, IStringDeserializer<Dictionary<int, EncounterBasicStatus>> parser)
        {
            this.urlBuilder = urlBuilder;
            this.serverReader = serverReader;
            this.parser = parser;
        }

        public WaitableTask<Dictionary<int, EncounterBasicStatus>> GetBasicStatuses(User user)
        {
            if (user.IsGuest)
                return new WaitableTask<Dictionary<int, EncounterBasicStatus>>(new Exception("Guest user has no server statuses."));

            var webRequest = GetWebRequest(user);
            var serverOutput = serverReader.Begin(webRequest);
            var statuses = new WaitableTask<Dictionary<int, EncounterBasicStatus>>();
            serverOutput.AddOnCompletedListener((result) => ProcessResults(statuses, result));

            return statuses;
        }

        private const string PhpFile = "DownloadStatus.php";
        private const string ModeVariable = "mode";
        private const string ModeValue = "download";
        private const string AccountIdVariable = "accountId";
        protected UnityWebRequest GetWebRequest(User user)
        {
            var url = urlBuilder.BuildUrl(PhpFile);
            var form = new WWWForm();

            form.AddField(ModeVariable, ModeValue);
            form.AddField(AccountIdVariable, user.AccountId);

            return UnityWebRequest.Post(url, form);
        }
        private void ProcessResults(WaitableTask<Dictionary<int, EncounterBasicStatus>> result, TaskResult<string> serverOutput)
        {
            if (serverOutput == null || serverOutput.IsError()) {
                result.SetError(serverOutput.Exception);
                return;
            }

            var statuses = parser.Deserialize(serverOutput.Value);
            result.SetResult(statuses);
        }
    }
}
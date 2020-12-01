
using System.Collections.Generic;
using UnityEngine.Networking;

namespace ClinicalTools.SimEncounters
{
    public class ServerMetadatasReader : IMetadatasReader
    {
        private readonly IUrlBuilder urlBuilder;
        private readonly IServerReader serverReader;
        private readonly IStringDeserializer<List<EncounterMetadata>> parser;
        public ServerMetadatasReader(IUrlBuilder urlBuilder, IServerReader serverReader, IStringDeserializer<List<EncounterMetadata>> parser)
        {
            this.urlBuilder = urlBuilder;
            this.serverReader = serverReader;
            this.parser = parser;
        }

        public WaitableTask<List<EncounterMetadata>> GetMetadatas(User user)
        {
            var webRequest = GetWebRequest(user);
            var serverOutput = serverReader.Begin(webRequest);
            var metadatas = new WaitableTask<List<EncounterMetadata>>();
            serverOutput.AddOnCompletedListener((result) => ProcessResults(metadatas, result));

            return metadatas;
        }

        private const string MENU_PHP = "DownloadEncounters.php";
        private const string MODE_VARIABLE = "mode";
        private const string MODE_VALUE = "downloadForOneAccount";
        private const string ACCOUNT_VARIABLE = "accountId";

        private UnityWebRequest GetWebRequest(User user)
        {
            var arguments = new UrlArgument[] {
                new UrlArgument(MODE_VARIABLE, MODE_VALUE),
                new UrlArgument(ACCOUNT_VARIABLE, user.AccountId.ToString())
            };
            var url = urlBuilder.BuildUrl(MENU_PHP, arguments);
            return UnityWebRequest.Get(url);
        }


        private void ProcessResults(WaitableTask<List<EncounterMetadata>> result, TaskResult<string> serverOutput)
        {
            if (serverOutput == null || serverOutput.IsError()) {
                result.SetError(serverOutput.Exception);
                return;
            }

            var metadatas = parser.Deserialize(serverOutput.Value);
            result.SetResult(metadatas);
        }
    }
}

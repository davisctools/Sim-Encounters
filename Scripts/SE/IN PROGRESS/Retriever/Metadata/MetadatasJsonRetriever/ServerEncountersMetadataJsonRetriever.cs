using SimpleJSON;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace ClinicalTools.SimEncounters
{
    public class ServerEncountersMetadataJsonRetriever : IEncountersMetadataJsonRetriever
    {
        protected IUrlBuilder UrlBuilder { get; }
        protected IServerStringReader ServerReader { get; }
        public ServerEncountersMetadataJsonRetriever(IUrlBuilder urlBuilder, IServerStringReader serverReader)
        {
            UrlBuilder = urlBuilder;
            ServerReader = serverReader;
        }

        public virtual WaitableTask<IEnumerable<JSONNode>> GetMetadataJsonNodes(User user)
        {
            var webRequest = GetWebRequest(user);
            var serverOutput = ServerReader.Begin(webRequest);
            var metadataNodes = new WaitableTask<IEnumerable<JSONNode>>();
            serverOutput.AddOnCompletedListener((result) => ProcessResults(metadataNodes, result, user));

            return metadataNodes;
        }

        protected virtual string MenuPhp { get; } = "Main.php";
        protected virtual string ModeVariable { get; } = "mode";
        protected virtual string ModeValue { get; } = "encounters";
        protected virtual string AccountVariable { get; } = "account";

        protected virtual UnityWebRequest GetWebRequest(User user)
        {
            var arguments = new UrlArgument[] {
                new UrlArgument(ModeVariable, ModeValue),
                new UrlArgument(AccountVariable, user.AccountId.ToString())
            };
            var url = UrlBuilder.BuildUrl(MenuPhp, arguments);
            return UnityWebRequest.Get(url);
        }


        protected virtual void ProcessResults(WaitableTask<IEnumerable<JSONNode>> result, TaskResult<string> serverOutput, User user)
        {
            if (serverOutput == null || serverOutput.IsError()) {
                result.SetError(serverOutput.Exception);
                return;
            }

            var root = JSON.Parse(serverOutput.Value);
            var encountersNode = root["encounters"];
            if (encountersNode == null)
                encountersNode = root;

            result.SetResult(encountersNode.Children);
        }
    }
}

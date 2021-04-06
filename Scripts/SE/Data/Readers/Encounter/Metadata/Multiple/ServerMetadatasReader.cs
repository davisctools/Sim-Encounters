using SimpleJSON;
using System;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace ClinicalTools.SimEncounters
{
    public class ServerMetadatasReader : IMetadatasReader
    {
        protected IUrlBuilder UrlBuilder { get; }
        protected IServerStringReader ServerReader { get; }
        protected IJsonDeserializer<EncounterMetadata> Parser { get; }
        protected IServerEncounterImageReader ServerImageReader { get; }
        public ServerMetadatasReader(
            IUrlBuilder urlBuilder, 
            IServerStringReader serverReader,
            IJsonDeserializer<EncounterMetadata> parser,
            IServerEncounterImageReader serverImageReader)
        {
            UrlBuilder = urlBuilder;
            ServerReader = serverReader;
            Parser = parser;
            ServerImageReader = serverImageReader;
        }

        public virtual WaitableTask<List<EncounterMetadata>> GetMetadatas(User user)
        {
            var webRequest = GetWebRequest(user);
            var serverOutput = ServerReader.Begin(webRequest);
            var metadatas = new WaitableTask<List<EncounterMetadata>>();
            serverOutput.AddOnCompletedListener((result) => ProcessResults(metadatas, result, user));

            return metadatas;
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


        protected virtual void ProcessResults(WaitableTask<List<EncounterMetadata>> result, TaskResult<string> serverOutput, User user)
        {
            if (serverOutput == null || serverOutput.IsError()) {
                result.SetError(serverOutput.Exception);
                return;
            }

            var metadatas = new List<EncounterMetadata>();
            var json = JSON.Parse(serverOutput.Value);
            var encountersNode = json["encounters"];
            foreach (var encounterNode in encountersNode)
                metadatas.Add(Parser.Deserialize(encounterNode));

            GetImages(result, metadatas, user);
        }

        protected virtual void GetImages(
            WaitableTask<List<EncounterMetadata>> result, 
            List<EncounterMetadata> metadatas, 
            User user)
        {
            var tasks = new List<WaitableTask>();
            foreach (var metadata in metadatas) {
                if (metadata.Image != null)
                    tasks.Add(ServerImageReader.GetTexture(user, metadata, metadata.Image));
            }

            void completionAction() => result.SetResult(metadatas);
            if (tasks.Count == 0) {
                completionAction();
                return;
            }

            var i = tasks.Count;
            foreach (var task in tasks)
                task.AddOnCompletedListener((r) => DecrementUntilZero(completionAction, ref i));
        }

        protected virtual void DecrementUntilZero(Action completionAction, ref int i)
        {
            if (--i == 0) completionAction();
        }
    }
}

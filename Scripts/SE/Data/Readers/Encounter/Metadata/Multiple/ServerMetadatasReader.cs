using SimpleJSON;
using System;
using System.Collections.Generic;
using UnityEngine;
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

        public WaitableTask<List<EncounterMetadata>> GetMetadatas(User user)
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

        private UnityWebRequest GetWebRequest(User user)
        {
            var arguments = new UrlArgument[] {
                new UrlArgument(ModeVariable, ModeValue),
                new UrlArgument(AccountVariable, user.AccountId.ToString())
            };
            var url = UrlBuilder.BuildUrl(MenuPhp, arguments);
            return UnityWebRequest.Get(url);
        }


        private void ProcessResults(WaitableTask<List<EncounterMetadata>> result, TaskResult<string> serverOutput, User user)
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

        private void GetImages(WaitableTask<List<EncounterMetadata>> result, List<EncounterMetadata> metadatas, User user)
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

            int i = tasks.Count;
            foreach (var task in tasks)
                task.AddOnCompletedListener((r) => DecrementUntilZero(completionAction, ref i));
        }

        protected virtual void DecrementUntilZero(
            Action completionAction,
            ref int i)
        {
            if (--i == 0)
                completionAction();
        }
    }

    public interface IServerEncounterImageReader
    {
        WaitableTask GetTexture(User user, EncounterMetadata metadata, EncounterImage image);
    }

    public class ServerEncounterImageReader : IServerEncounterImageReader
    {
        private readonly IUrlBuilder urlBuilder;
        private readonly IServerTextureReader serverReader;
        public ServerEncounterImageReader(IUrlBuilder urlBuilder, IServerTextureReader serverReader)
        {
            this.urlBuilder = urlBuilder;
            this.serverReader = serverReader;
        }

        public WaitableTask GetTexture(User user, EncounterMetadata metadata, EncounterImage image)
        {
            if (metadata.Image?.Sprite != null && metadata.Image.Id == image.Id) {
                image.Sprite = metadata.Image.Sprite;
                return WaitableTask.CompletedTask;
            }

            var webRequest = GetWebRequest(user, metadata, image.FileName);
            var serverOutput = serverReader.Begin(webRequest);
            var task = new WaitableTask();
            serverOutput.AddOnCompletedListener((result) => ProcessResults(task, image, result));

            return task;
        }


        protected virtual void ProcessResults(WaitableTask task, EncounterImage image, TaskResult<Texture2D> serverOutput)
        {
            if (serverOutput == null || serverOutput.IsError()) {
                task.SetError(serverOutput.Exception);
                return;
            }

            var texture = serverOutput.Value;
            if (texture == null) {
                task.SetError(new Exception("Could not get texture."));
                return;
            }
            image.Sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(texture.width / 2, texture.height / 2));
            task.SetCompleted();
        }

        protected virtual string MenuPhp { get; } = "Main.php";
        protected virtual string ModeVariable { get; } = "mode";
        protected virtual string ModeValue { get; } = "encounter";
        protected virtual string AccountVariable { get; } = "account";
        protected virtual string EncounterVariable { get; } = "encounter";
        protected virtual string ActionVariable { get; } = "action";
        protected virtual string ActionValue { get; } = "image";
        protected virtual string FileVariable { get; } = "file";

        protected virtual UnityWebRequest GetWebRequest(User user, EncounterMetadata metadata, string textureName)
        {
            var arguments = new UrlArgument[] {
                new UrlArgument(ModeVariable, ModeValue),
                new UrlArgument(AccountVariable, user.AccountId.ToString()),
                new UrlArgument(EncounterVariable, metadata.RecordNumber.ToString()),
                new UrlArgument(ActionVariable, ActionValue),
                new UrlArgument(FileVariable, textureName)
            };
            var url = urlBuilder.BuildUrl(MenuPhp, arguments);
            return UnityWebRequestTexture.GetTexture(url);
        }
    }
}

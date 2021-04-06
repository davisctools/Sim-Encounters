using System;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace ClinicalTools.SimEncounters
{
    public class ServerEncounterImagesReader : IServerEncounterImagesReader
    {
        protected IServerStringReader ServerReader { get; }
        protected IUrlBuilder UrlBuilder { get; }
        protected IStringDeserializer<List<EncounterImage>> Parser { get; }
        protected IServerEncounterImageReader ServerImageReader { get; }
        public ServerEncounterImagesReader(
            IServerStringReader serverReader,
            IUrlBuilder urlBuilder,
            IStringDeserializer<List<EncounterImage>> parser,
            IServerEncounterImageReader serverImageReader)
        {
            ServerReader = serverReader;
            UrlBuilder = urlBuilder;
            Parser = parser;
            ServerImageReader = serverImageReader;
        }

        public virtual WaitableTask<List<EncounterImage>> GetImages(User user, EncounterMetadata metadata)
        {
            var images = new WaitableTask<List<EncounterImage>>();

            var webRequest = GetWebRequest(user, metadata);
            var serverResult = ServerReader.Begin(webRequest);
            serverResult.AddOnCompletedListener((result) => ProcessResults(images, user, metadata, result));

            return images;
        }

        protected virtual string DownloadPhp { get; } = "Main.php";
        protected virtual string ModeVariable { get; } = "mode";
        protected virtual string ModeValue { get; } = "encounter";
        protected virtual string ActionVariable { get; } = "action";
        protected virtual string ActionValue { get; } = "images";
        protected virtual string EncounterVariable { get; } = "encounter";
        protected virtual string AccountVariable { get; } = "account";

        protected virtual UnityWebRequest GetWebRequest(User user, EncounterMetadata metadata)
        {
            var arguments = new UrlArgument[] {
                new UrlArgument(AccountVariable, user.AccountId.ToString()),
                new UrlArgument(ModeVariable, ModeValue),
                new UrlArgument(ActionVariable, ActionValue),
                new UrlArgument(EncounterVariable, metadata.RecordNumber.ToString()),
            };
            var url = UrlBuilder.BuildUrl(DownloadPhp, arguments);
            return UnityWebRequest.Get(url);
        }

        protected virtual void ProcessResults(
            WaitableTask<List<EncounterImage>> imagesTask,
            User user,
            EncounterMetadata metadata,
            TaskResult<string> serverResult)
        {
            if (serverResult.IsError()) {
                imagesTask.SetError(serverResult.Exception);
                return;
            }

            var images = Parser.Deserialize(serverResult.Value);
            var tasks = new List<WaitableTask>();
            foreach (var image in images)
                tasks.Add(ServerImageReader.GetTexture(user, metadata, image));

            void completionAction() => imagesTask.SetResult(images);
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
}
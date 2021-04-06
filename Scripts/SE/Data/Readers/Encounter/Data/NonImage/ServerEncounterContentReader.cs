using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace ClinicalTools.SimEncounters
{
    public class ServerEncounterContentReader : IEncounterDataReader
    {
        protected IServerStringReader ServerReader { get; }
        protected IUrlBuilder UrlBuilder { get; }
        protected IStringDeserializer<EncounterContent> Parser { get; }
        protected IServerEncounterImagesReader ServerEncounterImagesReader { get; }
        public ServerEncounterContentReader(
            IServerStringReader serverReader,
            IUrlBuilder urlBuilder,
            IStringDeserializer<EncounterContent> parser,
            IServerEncounterImagesReader serverEncounterImagesReader)
        {
            ServerReader = serverReader;
            UrlBuilder = urlBuilder;
            Parser = parser;
            ServerEncounterImagesReader = serverEncounterImagesReader;
        }

        public virtual WaitableTask<EncounterContent> GetEncounterData(User user, EncounterMetadata metadata)
        {
            var mainTask = new WaitableTask<EncounterContent>();

            var webRequest = GetWebRequest(user, metadata);
            var serverResult = ServerReader.Begin(webRequest);
            var contentTask = new WaitableTask<EncounterContent>();
            var imagesTask = ServerEncounterImagesReader.GetImages(user, metadata);
            serverResult.AddOnCompletedListener((result) => ProcessResults(contentTask, result));

            void setMainTask() => SetMainTask(mainTask, contentTask, imagesTask, metadata);
            contentTask.AddOnCompletedListener((result) => setMainTask());
            imagesTask.AddOnCompletedListener((result) => setMainTask());

            return mainTask;
        }

        protected virtual void SetMainTask(
            WaitableTask<EncounterContent> mainTask,
            WaitableTask<EncounterContent> contentTask,
            WaitableTask<List<EncounterImage>> imagesTask,
            EncounterMetadata metadata)
        {
            if (mainTask.IsCompleted() || !contentTask.IsCompleted())
                return;
            else if (contentTask.Result.IsError())
                mainTask.SetError(contentTask.Result.Exception);
            else if (!imagesTask.IsCompleted())
                return;
            else if (imagesTask.Result.IsError())
                mainTask.SetResult(contentTask.Result.Value);

            var content = contentTask.Result.Value;
            foreach (var image in imagesTask.Result.Value) {
                if (content.Images.ContainsKey(image.Key))
                    Debug.LogError($"Duplicate image key ({image.Key}: ID {image.Id} and {content.Images[image.Key].Id})");
                else
                    content.Images.AddKeyedValue(image.Key, image);

                if (image.Key == metadata.Image.Key)
                    metadata.Image = image;
            }
            mainTask.SetResult(content);
        }

        protected virtual string DownloadPhp { get; } = "Main.php";
        protected virtual string ModeVariable { get; } = "mode";
        protected virtual string ModeValue { get; } = "encounter";
        protected virtual string ActionVariable { get; } = "action";
        protected virtual string ActionValue { get; } = "data";
        protected virtual string EncounterVariable { get; } = "encounter";
        protected virtual string AccountVariable { get; } = "account";
        private UnityWebRequest GetWebRequest(User user, EncounterMetadata metadata)
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

        private void ProcessResults(WaitableTask<EncounterContent> result, TaskResult<string> serverResult)
        {
            result.SetResult(Parser.Deserialize(serverResult.Value));
        }
    }
}
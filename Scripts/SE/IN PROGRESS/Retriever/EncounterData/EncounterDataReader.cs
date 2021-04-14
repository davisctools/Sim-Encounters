using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class EncounterDataReader : IEncounterDataReader
    {
        protected IServerStringReader ServerReader { get; }
        protected IUrlBuilder UrlBuilder { get; }
        protected IEncounterDataTextRetriever TextRetriever { get; }
        protected IStringDeserializer<EncounterContentData> Parser { get; }
        protected IEncounterImagesReader ServerEncounterImagesReader { get; }
        public EncounterDataReader(
            IServerStringReader serverReader,
            IUrlBuilder urlBuilder,
            IEncounterDataTextRetriever textRetriever,
            IStringDeserializer<EncounterContentData> parser,
            IEncounterImagesReader serverEncounterImagesReader)
        {
            ServerReader = serverReader;
            UrlBuilder = urlBuilder;
            TextRetriever = textRetriever;
            Parser = parser;
            ServerEncounterImagesReader = serverEncounterImagesReader;
        }

        public virtual WaitableTask<EncounterContentData> GetEncounterData(User user, OldEncounterMetadata metadata)
        {
            var mainTask = new WaitableTask<EncounterContentData>();

            var encounterContentTextTask = TextRetriever.GetDataText(user, metadata);
            var contentTask = new WaitableTask<EncounterContentData>();
            var imagesTask = ServerEncounterImagesReader.GetImages(user, metadata);
            encounterContentTextTask.AddOnCompletedListener((result) => ProcessResults(contentTask, result));

            void setMainTask() => SetMainTask(mainTask, contentTask, imagesTask, metadata);
            contentTask.AddOnCompletedListener((result) => setMainTask());
            imagesTask.AddOnCompletedListener((result) => setMainTask());

            return mainTask;
        }

        protected virtual void SetMainTask(
            WaitableTask<EncounterContentData> mainTask,
            WaitableTask<EncounterContentData> contentTask,
            WaitableTask<List<EncounterImage>> imagesTask,
            OldEncounterMetadata metadata)
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

        protected virtual void ProcessResults(WaitableTask<EncounterContentData> result, TaskResult<string> serverResult)
        {
            result.SetResult(Parser.Deserialize(serverResult.Value));
        }
    }
}
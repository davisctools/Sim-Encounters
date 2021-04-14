﻿using System;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public class EncounterImagesReader : IEncounterImagesReader
    {
        protected IStringDeserializer<List<EncounterImage>> Parser { get; }
        protected IEncounterImageSpriteRefresher ServerImageReader { get; }
        protected IEncounterImagesJsonRetriever EncounterImagesJsonRetriever { get; }
        public EncounterImagesReader(
            IEncounterImagesJsonRetriever encounterImagesJsonRetriever,
            IStringDeserializer<List<EncounterImage>> parser,
            IEncounterImageSpriteRefresher serverImageReader)
        {
            EncounterImagesJsonRetriever = encounterImagesJsonRetriever;
            Parser = parser;
            ServerImageReader = serverImageReader;
        }

        public virtual WaitableTask<List<EncounterImage>> GetImages(User user, OldEncounterMetadata metadata)
        {
            var images = new WaitableTask<List<EncounterImage>>();

            var imagesJson = EncounterImagesJsonRetriever.GetImagesJson(user, metadata);
            imagesJson.AddOnCompletedListener((result) => ProcessResults(images, user, metadata, result));

            return images;
        }

        protected virtual void ProcessResults(
            WaitableTask<List<EncounterImage>> imagesTask,
            User user,
            OldEncounterMetadata metadata,
            TaskResult<string> serverResult)
        {
            if (serverResult.IsError()) {
                imagesTask.SetError(serverResult.Exception);
                return;
            }

            var images = Parser.Deserialize(serverResult.Value);
            var tasks = new List<WaitableTask>();
            foreach (var image in images)
                tasks.Add(ServerImageReader.RefreshTexture(user, metadata, image));

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
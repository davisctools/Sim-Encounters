using SimpleJSON;
using System;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public class EncountersMetadataReader : IMetadatasReader
    {
        protected IEncountersMetadataJsonRetriever EncountersMetadataJsonRetriever { get; }
        protected IJsonDeserializer<OldEncounterMetadata> Parser { get; }
        protected IEncounterImageSpriteRefresher ServerImageReader { get; }
        public EncountersMetadataReader(
            IEncountersMetadataJsonRetriever encountersMetadataJsonRetriever,
            IJsonDeserializer<OldEncounterMetadata> parser,
            IEncounterImageSpriteRefresher serverImageReader)
        {
            EncountersMetadataJsonRetriever = encountersMetadataJsonRetriever;
            Parser = parser;
            ServerImageReader = serverImageReader;
        }

        public WaitableTask<List<OldEncounterMetadata>> GetMetadatas(User user)
        {
            var mainTask = new WaitableTask<List<OldEncounterMetadata>>();

            var encountersMetadataJsonTask = EncountersMetadataJsonRetriever.GetMetadataJsonNodes(user);
            encountersMetadataJsonTask.AddOnCompletedListener((result) => ProcessResults(mainTask, result, user));
            return null;
        }
        protected virtual void ProcessResults(WaitableTask<List<OldEncounterMetadata>> result, TaskResult<IEnumerable<JSONNode>> encountersMetadataJson, User user)
        {
            if (encountersMetadataJson == null || encountersMetadataJson.IsError()) {
                result.SetError(encountersMetadataJson.Exception);
                return;
            }

            var metadatas = new List<OldEncounterMetadata>();
            foreach (var encounterNode in encountersMetadataJson.Value)
                metadatas.Add(Parser.Deserialize(encounterNode));

            GetImages(result, metadatas, user);
        }

        protected virtual void GetImages(
            WaitableTask<List<OldEncounterMetadata>> result,
            List<OldEncounterMetadata> metadatas,
            User user)
        {
            var tasks = new List<WaitableTask>();
            foreach (var metadata in metadatas) {
                if (metadata.Image != null)
                    tasks.Add(ServerImageReader.RefreshTexture(user, metadata, metadata.Image));
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

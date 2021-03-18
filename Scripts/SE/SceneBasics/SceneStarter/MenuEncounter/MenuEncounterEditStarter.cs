using ClinicalTools.UI;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public class MenuEncounterEditStarter : IMenuEncounterStarter
    {
        protected BaseMessageHandler MessageHandler { get; set; }
        protected IEncounterLocker EncounterLocker { get; set; }
        protected IWriterSceneStarter SceneStarter { get; set; }
        protected IEncounterReader EncounterReader { get; set; }
        protected BaseMenuEncounterMetadataSelector MetadataSelector { get; set; }
        public MenuEncounterEditStarter(
            BaseMessageHandler messageHandler,
            IEncounterLocker encounterLocker,
            IWriterSceneStarter sceneStarter,
            IEncounterReader encounterReader,
            BaseMenuEncounterMetadataSelector metadataSelector)
        {
            MessageHandler = messageHandler;
            EncounterLocker = encounterLocker;
            SceneStarter = sceneStarter;
            EncounterReader = encounterReader;
            MetadataSelector = metadataSelector;
        }

        public virtual void StartEncounter(MenuSceneInfo sceneInfo, MenuEncounter menuEncounter)
            => SelectMetadata(sceneInfo, menuEncounter);

        protected virtual void SelectMetadata(MenuSceneInfo sceneInfo, MenuEncounter menuEncounter)
        {
            if (MetadataSelector == null) {
                EnsureEncounterUnlocked(sceneInfo, menuEncounter, menuEncounter.GetLatestTypedMetada());
                return;
            }

            var result = MetadataSelector.GetMetadata(menuEncounter);
            result.AddOnCompletedListener((value) => MetadataSelected(sceneInfo, menuEncounter, value));
        }

        protected virtual void MetadataSelected(
            MenuSceneInfo sceneInfo,
            MenuEncounter menuEncounter,
            TaskResult<KeyValuePair<SaveType, EncounterMetadata>> metadata)
        {
            if (metadata.HasValue())
                EnsureEncounterUnlocked(sceneInfo, menuEncounter, metadata.Value);
        }

        protected virtual void EnsureEncounterUnlocked(
            MenuSceneInfo sceneInfo,
            MenuEncounter menuEncounter,
            KeyValuePair<SaveType, EncounterMetadata> metadata)
        {
            if (menuEncounter.Metadata.ContainsKey(SaveType.Server)) {
                var task = EncounterLocker.LockEncounter(sceneInfo.User, metadata.Value);
                task.AddOnCompletedListener((result) => EncounterLocked(result, sceneInfo, metadata));
            } else {
                StartWriter(sceneInfo, metadata);
            }
        }

        protected virtual void EncounterLocked(
            TaskResult result,
            MenuSceneInfo sceneInfo,
            KeyValuePair<SaveType, EncounterMetadata> metadata)
        {
            if (!result.IsError()) {
                StartWriter(sceneInfo, metadata);
                return;
            }

            MessageHandler.ShowMessage($"Cannot set lock on encounter: {result.Exception.Message}", MessageType.Error);
        }

        protected virtual void StartWriter(MenuSceneInfo sceneInfo, KeyValuePair<SaveType, EncounterMetadata> metadata)
        {
            if (metadata.Value == null)
                return;

            var encounter = EncounterReader.GetEncounter(sceneInfo.User, metadata.Value, metadata.Key);
            var encounterSceneInfo = new LoadingWriterSceneInfo(sceneInfo.User, sceneInfo.LoadingScreen, encounter);
            SceneStarter.StartScene(encounterSceneInfo);
        }
    }
}
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
            => EnsureEncounterUnlocked(sceneInfo, menuEncounter);

        protected virtual void EnsureEncounterUnlocked(MenuSceneInfo sceneInfo, MenuEncounter menuEncounter)
        {
            var task = EncounterLocker.LockEncounter(sceneInfo.User, menuEncounter.GetLatestMetadata());
            task.AddOnCompletedListener((result) => EncounterLocked(result, sceneInfo, menuEncounter));
        }

        protected virtual void EncounterLocked(TaskResult result, MenuSceneInfo sceneInfo, MenuEncounter menuEncounter)
        {
            if (!result.IsError()) {
                SelectMetadata(sceneInfo, menuEncounter);
                return;
            }

            MessageHandler.ShowMessage("Cannot set lock on encounter.", MessageType.Error);
        }


        protected virtual void SelectMetadata(MenuSceneInfo sceneInfo, MenuEncounter menuEncounter)
        {
            if (MetadataSelector == null) {
                MetadataSelected(sceneInfo, menuEncounter.GetLatestTypedMetada());
                return;
            }

            var result = MetadataSelector.GetMetadata(menuEncounter);
            result.AddOnCompletedListener((value) => MetadataSelected(sceneInfo, value));
        }

        protected virtual void MetadataSelected(MenuSceneInfo sceneInfo, TaskResult<KeyValuePair<SaveType, EncounterMetadata>> metadata)
            => MetadataSelected(sceneInfo, metadata.Value);
        protected virtual void MetadataSelected(MenuSceneInfo sceneInfo,  KeyValuePair<SaveType, EncounterMetadata> metadata)
        {
            if (metadata.Value == null)
                return;

            var encounter = EncounterReader.GetEncounter(sceneInfo.User, metadata.Value, metadata.Key);
            var encounterSceneInfo = new LoadingWriterSceneInfo(sceneInfo.User, sceneInfo.LoadingScreen, encounter);
            SceneStarter.StartScene(encounterSceneInfo);
        }
    }
}
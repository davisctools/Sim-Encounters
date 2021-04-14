using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public class MenuEncounterReadStarter : IMenuEncounterStarter
    {
        protected IReaderSceneStarter SceneStarter { get; set; }
        protected IUserEncounterReader EncounterReader { get; set; }
        protected BaseMenuEncounterMetadataSelector MetadataSelector { get; set; }
        public MenuEncounterReadStarter(IReaderSceneStarter sceneStarter, IUserEncounterReader encounterReader, BaseMenuEncounterMetadataSelector metadataSelector)
        {
            SceneStarter = sceneStarter;
            EncounterReader = encounterReader;
            MetadataSelector = metadataSelector;
        }

        public virtual void StartEncounter(MenuSceneInfo sceneInfo, MenuEncounter menuEncounter)
        {
            if (MetadataSelector == null) {
                MetadataSelected(sceneInfo, menuEncounter.Status, menuEncounter.GetLatestTypedMetada());
                return;
            }

            var result = MetadataSelector.GetMetadata(menuEncounter);
            result.AddOnCompletedListener((value) => MetadataSelected(sceneInfo, menuEncounter.Status, value));
        }

        protected virtual void MetadataSelected(MenuSceneInfo sceneInfo, EncounterBasicStatus status, TaskResult<KeyValuePair<SaveType, OldEncounterMetadata>> metadata)
            => MetadataSelected(sceneInfo, status, metadata.Value);

        protected virtual void MetadataSelected(MenuSceneInfo sceneInfo, EncounterBasicStatus status, KeyValuePair<SaveType, OldEncounterMetadata> metadata)
        {
            if (status == null)
                status = new EncounterBasicStatus();

            var encounter = EncounterReader.GetUserEncounter(sceneInfo.User, metadata.Value, status, metadata.Key);
            var encounterSceneInfo = new LoadingReaderSceneInfo(sceneInfo.User, sceneInfo.LoadingScreen, encounter);
            SceneStarter.StartScene(encounterSceneInfo);
        }
    }
}
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public class MenuEncounterEditStarter : IMenuEncounterStarter
    {
        protected IWriterSceneStarter SceneStarter { get; set; }
        protected IEncounterReader EncounterReader { get; set; }
        protected BaseMenuEncounterMetadataSelector MetadataSelector { get; set; }
        public MenuEncounterEditStarter(IWriterSceneStarter sceneStarter, IEncounterReader encounterReader, BaseMenuEncounterMetadataSelector metadataSelector)
        {
            SceneStarter = sceneStarter;
            EncounterReader = encounterReader;
            MetadataSelector = metadataSelector;
        }

        public virtual void StartEncounter(MenuSceneInfo sceneInfo, MenuEncounter menuEncounter)
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
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class WriterSceneManager : SceneManager<LoadingWriterSceneInfo>, ILoadingWriterSceneDrawer
    {
        public string DefaultEncounterFileName { get => defaultEncounterFileName; set => defaultEncounterFileName = value; }
        [SerializeField] private string defaultEncounterFileName;

        protected ISelector<LoadingWriterSceneInfoSelectedEventArgs> SceneSelector { get; set; }
        protected IMetadataReader MetadataReader { get; set; }
        protected IEncounterReader EncounterReader { get; set; }
        [Inject]
        public virtual void Inject(
            ISelector<LoadingWriterSceneInfoSelectedEventArgs> sceneSelector,
            IMetadataReader metadataReader, 
            IEncounterReader encounterReader)
        {
            SceneSelector = sceneSelector;
            MetadataReader = metadataReader;
            EncounterReader = encounterReader;
        }
        protected override void StartAsInitialScene()
        {
            var tempMetadata = new EncounterMetadata() {
                Filename = DefaultEncounterFileName
            };
            var metadataResult = MetadataReader.GetMetadata(User.Guest, tempMetadata);
            metadataResult.AddOnCompletedListener(MetadataRetrieved);
        }

        public virtual void MetadataRetrieved(TaskResult<EncounterMetadata> metadata)
        {
            if (!metadata.HasValue()) {
                Debug.LogError("Metadata is null.");
                return;
            }

            var encounter = EncounterReader.GetEncounter(User.Guest, metadata.Value, SaveType.Demo);
            var sceneInfo = new LoadingWriterSceneInfo(User.Guest, null, encounter);
            Display(sceneInfo);
        }

        protected override void StartAsLaterScene() { }


        protected override void ProcessSceneInfo(LoadingWriterSceneInfo sceneInfo)
        {
            SceneSelector.Select(this, new LoadingWriterSceneInfoSelectedEventArgs(sceneInfo));
            sceneInfo.Result.AddOnCompletedListener(SceneInfoLoaded);
        }
        protected virtual void SceneInfoLoaded(TaskResult<WriterSceneInfo> sceneInfo)
        {
            if (!sceneInfo.HasValue())
                return;
            if (sceneInfo.Value.LoadingScreen != null)
                sceneInfo.Value.LoadingScreen.Stop();
        }
    }
}
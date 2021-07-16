#if DEEP_LINKING
using ImaginationOverflow.UniversalDeepLinking;
#endif
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ReaderManager : SceneManager<LoadingReaderSceneInfo>, IReaderSceneDrawer
    {
        public string DefaultEncounterFileName { get => defaultEncounterFileName; set => defaultEncounterFileName = value; }
        [SerializeField] private string defaultEncounterFileName;
        public LoadingScreen LoadingScreen { get => loadingScreen; set => loadingScreen = value; }
        [SerializeField] private LoadingScreen loadingScreen;

        public List<GameObject> StandaloneSceneObjects { get => standaloneSceneObjects; set => standaloneSceneObjects = value; }
        [SerializeField] private List<GameObject> standaloneSceneObjects;

        public List<GameObject> NonStandaloneSceneObjects { get => nonStandaloneSceneObjects; set => nonStandaloneSceneObjects = value; }
        [SerializeField] private List<GameObject> nonStandaloneSceneObjects;

        protected ISelector<LoadingReaderSceneInfoSelectedEventArgs> SceneSelector { get; set; }
        protected IMetadataReader MetadataReader { get; set; }
        protected IUserEncounterReader EncounterReader { get; set; }
        [Inject]
        public virtual void Inject(
            ISelector<LoadingReaderSceneInfoSelectedEventArgs> loadingSceneInfoSelector,
            IMetadataReader metadataReader,
            IUserEncounterReader encounterReader)
        {
            SceneSelector = loadingSceneInfoSelector;
            MetadataReader = metadataReader;
            EncounterReader = encounterReader;

        }

#if DEEP_LINKING
        protected IEncounterQuickStarter EncounterQuickStarter { get; set; }
        protected QuickActionFactory LinkActionFactory { get; set; }
        [Inject]
        public virtual void Inject(QuickActionFactory linkActionFactory, IEncounterQuickStarter encounterQuickStarter)
        {
            LinkActionFactory = linkActionFactory;
            EncounterQuickStarter = encounterQuickStarter;
        }
#endif

        protected override void Awake()
        {
            base.Awake();

#if DEEP_LINKING
            DeepLinkManager.Instance.LinkActivated += Instance_LinkActivated;
#endif
        }

        private bool started;
        protected override void Start()
        {
            base.Start();
            started = true;
            if (SceneInfo != null)
                SceneSelector.Select(this, new LoadingReaderSceneInfoSelectedEventArgs(SceneInfo));
        }

        protected override void StartAsInitialScene()
        {
            Screen.fullScreen = false;

            var tempMetadata = new EncounterMetadata() {
                Filename = DefaultEncounterFileName
            };
            var metadataResult = MetadataReader.GetMetadata(User.Guest, tempMetadata);
            metadataResult.AddOnCompletedListener(MetadataRetrieved);

#if STANDALONE_SCENE
            foreach (var standaloneSceneObject in StandaloneSceneObjects)
                standaloneSceneObject.SetActive(true);
            foreach (var nonStandaloneSceneObject in NonStandaloneSceneObjects)
                nonStandaloneSceneObject.SetActive(false);
#else
            foreach (var standaloneSceneObject in StandaloneSceneObjects)
                standaloneSceneObject.SetActive(false);
            foreach (var nonStandaloneSceneObject in NonStandaloneSceneObjects)
                nonStandaloneSceneObject.SetActive(true);
#endif
        }

        protected override void StartAsLaterScene()
        {
            foreach (var standaloneSceneObject in StandaloneSceneObjects)
                standaloneSceneObject.SetActive(false);
            foreach (var nonStandaloneSceneObject in NonStandaloneSceneObjects)
                nonStandaloneSceneObject.SetActive(true);

            Destroy(LoadingScreen.gameObject);
        }


        public virtual void MetadataRetrieved(TaskResult<EncounterMetadata> metadata)
        {
            if (metadata.Value == null) {
                Debug.LogError("Metadata is null.");
                return;
            }

            var fullEncounter = EncounterReader.GetUserEncounter(User.Guest, metadata.Value, new EncounterBasicStatus(), SaveType.Demo);
            var sceneInfo = new LoadingReaderSceneInfo(User.Guest, LoadingScreen, fullEncounter);

            Display(sceneInfo);
        }

        protected LoadingReaderSceneInfo SceneInfo { get; set; }
        protected override void ProcessSceneInfo(LoadingReaderSceneInfo sceneInfo)
        {
            SceneInfo = sceneInfo;

            if (started)
                SceneSelector.Select(this, new LoadingReaderSceneInfoSelectedEventArgs(SceneInfo));
            sceneInfo.Result.AddOnCompletedListener(SceneInfoLoaded);

#if DEEP_LINKING
            if (onLoadAction != null)
                EncounterQuickStarter.StartEncounter(SceneInfo.User, SceneInfo.LoadingScreen, onLoadAction.EncounterId);
#endif
        }
        protected virtual void SceneInfoLoaded(TaskResult<ReaderSceneInfo> sceneInfo)
        {
            if (!sceneInfo.HasValue())
                return;
            if (sceneInfo.Value.LoadingScreen != null)
                sceneInfo.Value.LoadingScreen.Stop();
        }


#if DEEP_LINKING
        protected virtual void OnDestroy() => DeepLinkManager.Instance.LinkActivated -= Instance_LinkActivated;

        public void TestLink()
        {
            var idNum = "73";
            var dictionary = new Dictionary<string, string>();
            dictionary.Add(IdKey, idNum);
            var linkActivation = new LinkActivation($"lift://encounter?{IdKey}={idNum}", $"{IdKey}={idNum}", dictionary);

            Instance_LinkActivated(linkActivation);
        }
        private const string IdKey = "id";
        public void TestLink2()
        {
            var idNum = "95";
            var dictionary = new Dictionary<string, string>();
            dictionary.Add(IdKey, idNum);
            var linkActivation = new LinkActivation($"lift://encounter?{IdKey}={idNum}", $"{IdKey}={idNum}", dictionary);

            Instance_LinkActivated(linkActivation);
        }

        private QuickAction onLoadAction;
        protected virtual void Instance_LinkActivated(LinkActivation s)
        {
            QuickAction quickAction = LinkActionFactory.GetLinkAction(s);
            if (quickAction.Action == QuickActionType.NA)
                return;

            SceneInfo.Result.RemoveListeners();
            if (SceneInfo != null)
                EncounterQuickStarter.StartEncounter(SceneInfo.User, SceneInfo.LoadingScreen, quickAction.EncounterId);
            else
                onLoadAction = quickAction;
        }
#endif
    }
}
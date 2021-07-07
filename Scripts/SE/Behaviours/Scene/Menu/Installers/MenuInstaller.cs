using ClinicalTools.UI;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class MenuInstaller : MonoInstaller
    {
        public BaseConfirmationPopup ConfirmationPopup { get => confirmationPopup; set => confirmationPopup = value; }
        [SerializeField] private BaseConfirmationPopup confirmationPopup;
        public AnimationMonitor AnimationMonitor { get => animationMonitor; set => animationMonitor = value; }
        [SerializeField] private AnimationMonitor animationMonitor;
        public SwipeManager SwipeManager { get => swipeManager; set => swipeManager = value; }
        [SerializeField] private SwipeManager swipeManager;
        public ReaderSidebarController SidebarController { get => sidebarController; set => sidebarController = value; }
        [SerializeField] private ReaderSidebarController sidebarController;
        public EncounterFilterBehaviour Filters { get => filters; set => filters = value; }
        [SerializeField] private EncounterFilterBehaviour filters;
        public DeleteEncounterPopup DeleteEncounterPopup { get => deleteEncounterPopup; set => deleteEncounterPopup = value; }
        [SerializeField] private DeleteEncounterPopup deleteEncounterPopup;
        public BaseAddEncounterPopup AddEncounterPopup { get => addEncounterPopup; set => addEncounterPopup = value; }
        [SerializeField] private BaseAddEncounterPopup addEncounterPopup;
        public BaseMessageHandler MessageHandler { get => messageHandler; set => messageHandler = value; }
        [SerializeField] private BaseMessageHandler messageHandler;
        public BaseMenuEncounterMetadataSelector MetadataSelector { get => metadataSelector; set => metadataSelector = value; }
        [SerializeField] private BaseMenuEncounterMetadataSelector metadataSelector;
        public AndroidBackButton BackButton { get => backButton; set => backButton = value; }
        [SerializeField] private AndroidBackButton backButton;
        public MainMenuEncounterUI EncounterSelectorPrefab { get => encounterSelectorPrefab; set => encounterSelectorPrefab = value; }
        [SerializeField] private MainMenuEncounterUI encounterSelectorPrefab;
        public BaseMenuEncounterOverview MenuEncounterOverview { get => menuEncounterOverview; set => menuEncounterOverview = value; }
        [SerializeField] private BaseMenuEncounterOverview menuEncounterOverview;
        public Transform PoolParent { get => poolParent; set => poolParent = value; }
        [SerializeField] private Transform poolParent;

        public override void InstallBindings()
        {
            Container.Bind<IMenuEncounterStarter>().To<MenuEncounterEditStarter>().AsTransient().WhenInjectedInto<MenuEncounterWriterButtons>();
            Container.Bind<IMenuEncounterStarter>().To<MenuEncounterReadStarter>().AsTransient().WhenInjectedInto<MenuEncounterReaderButtons>();

            Container.BindInterfacesTo<LoadingMenuSceneInfoSelector>().AsSingle();
            Container.BindInterfacesTo<MenuSceneInfoSelector>().AsSingle();

            var fileManager = new FileManagerInstaller();
            fileManager.BindFileManagerWithId(Container, SaveType.Local);
            fileManager.BindFileManagerWithId(Container, SaveType.Autosave);
            Container.Bind<IEncounterRemover>().WithId(SaveType.Local).To<LocalEncounterRemover>().AsTransient();
            Container.Bind<IEncounterRemover>().WithId(SaveType.Server).To<ServerEncounterRemover>().AsTransient();

            Container.BindInstance<IDeleteEncounterHandler>(DeleteEncounterPopup);

            Container.BindInstance(SidebarController);
            Container.BindInstance(Filters);
            Container.BindInstance(AnimationMonitor);
            Container.BindInstance(SwipeManager);

            Container.BindInstance(ConfirmationPopup);
            Container.BindInstance(BackButton);
            Container.BindInstance(AddEncounterPopup);
            Container.BindInstance(MessageHandler);
            Container.BindInstance(MetadataSelector);
            Container.BindInstance(MenuEncounterOverview);

            Container.BindMemoryPool<MainMenuEncounterUI, MenuEncounterSelector.Pool>()
                .FromComponentInNewPrefab(EncounterSelectorPrefab)
                .UnderTransform(PoolParent);

            Container.DeclareSignal<EncounterLocksUpdatedSignal>().OptionalSubscriber();
        }
    }
}
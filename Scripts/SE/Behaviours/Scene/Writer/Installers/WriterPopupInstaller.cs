using ClinicalTools.UI;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{

    public class WriterPopupInstaller : MonoInstaller
    {
        public BaseCharactersEditor CharactersEditor { get => charactersEditor; set => charactersEditor = value; }
        [SerializeField] private BaseCharactersEditor charactersEditor;
        public BaseIconSelector IconSelector { get => iconSelector; set => iconSelector = value; }
        [SerializeField] private BaseIconSelector iconSelector;
        public BaseColorSelector ColorSelector { get => colorSelector; set => colorSelector = value; }
        [SerializeField] private BaseColorSelector colorSelector;

        public WriterDialoguePopup DialoguePopup { get => dialoguePopup; set => dialoguePopup = value; }
        [SerializeField] private WriterDialoguePopup dialoguePopup;
        public WriterQuizPopup QuizPopup { get => quizPopup; set => quizPopup = value; }
        [SerializeField] private WriterQuizPopup quizPopup;
        public SectionEditorPopup SectionEditorPopup { get => sectionEditorPopup; set => sectionEditorPopup = value; }
        [SerializeField] private SectionEditorPopup sectionEditorPopup;
        public TabEditorPopup TabEditorPopup { get => tabEditorPopup; set => tabEditorPopup = value; }
        [SerializeField] private TabEditorPopup tabEditorPopup;
        public BaseConfirmationPopup ConfirmationPopup { get => confirmationPopup; set => confirmationPopup = value; }
        [SerializeField] private BaseConfirmationPopup confirmationPopup;

        public EncounterSpriteUploader EncounterSpriteSelector { get => encounterSpriteSelector; set => encounterSpriteSelector = value; }
        [SerializeField] private EncounterSpriteUploader encounterSpriteSelector;
        public KeyedSpriteUploader KeyedSpriteSelector { get => keyedSpriteSelector; set => keyedSpriteSelector = value; }
        [SerializeField] private KeyedSpriteUploader keyedSpriteSelector;
        public SpriteUploader SpriteSelector { get => spriteSelector; set => spriteSelector = value; }
        [SerializeField] private SpriteUploader spriteSelector;

        public BaseMessageHandler MessageHandler { get => messageHandler; set => messageHandler = value; }
        [SerializeField] private BaseMessageHandler messageHandler;
        public SwipeManager SwipeManager { get => swipeManager; set => swipeManager = value; }
        [SerializeField] private SwipeManager swipeManager;
        public BaseSaveEncounterDisplay SaveEncounterDisplay { get => saveEncounterDisplay; set => saveEncounterDisplay = value; }
        [SerializeField] private BaseSaveEncounterDisplay saveEncounterDisplay;

        public override void InstallBindings()
        {
            Container.BindInstance(ColorSelector);
            Container.BindInstance(IconSelector);
            Container.Bind<IIconSpriteRetriever>().To<IconSpriteRetriever>().AsSingle();
            Container.BindInstance(CharactersEditor);

            Container.BindInstance(DialoguePopup);
            Container.BindInstance(QuizPopup);
            Container.BindInstance(SectionEditorPopup);
            Container.BindInstance(TabEditorPopup);
            Container.BindInstance(ConfirmationPopup);
            Container.BindInstance(SaveEncounterDisplay);

            Container.BindInstance<IEncounterSpriteSelector>(EncounterSpriteSelector);
            Container.BindInstance<IKeyedSpriteSelector>(KeyedSpriteSelector);
            Container.BindInstance<ISpriteSelector>(SpriteSelector);

            Container.BindInstance(SwipeManager);

            Container.BindInstance(MessageHandler);

            Container.BindInterfacesTo<EncounterSelectorManager>().AsSingle();
            Container.BindInterfacesTo<LoadingWriterSceneInfoSelector>().AsSingle();
            Container.BindInterfacesTo<WriterSceneInfoSelector>().AsSingle();
            Container.DeclareSignal<SerializeEncounterSignal>().OptionalSubscriber();
        }
    }

    public class SerializeEncounterSignal { }
}
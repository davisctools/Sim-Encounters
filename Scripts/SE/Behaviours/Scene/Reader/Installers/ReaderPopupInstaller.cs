using ClinicalTools.UI;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ReaderPopupInstaller : MonoInstaller
    {
        public BaseTooltip Tooltip { get => tooltip; set => tooltip = value; }
        [SerializeField] private BaseTooltip tooltip;
        public ReaderSidebarController SidebarController { get => sidebarController; set => sidebarController = value; }
        [SerializeField] private ReaderSidebarController sidebarController;
        public BaseInstructionsPopup InstructionsPopup { get => instructionsPopup; set => instructionsPopup = value; }
        [SerializeField] private BaseInstructionsPopup instructionsPopup;
        public BaseSettingsPopup SettingsPopup { get => settingsPopup; set => settingsPopup = value; }
        [SerializeField] private BaseSettingsPopup settingsPopup;
        public BaseReaderEncounterInfoPopup EncounterInfoPopup { get => encounterInfoPopup; set => encounterInfoPopup = value; }
        [SerializeField] private BaseReaderEncounterInfoPopup encounterInfoPopup;
        public BaseUserDialoguePinDrawer DialoguePopup { get => dialoguePopup; set => dialoguePopup = value; }
        [SerializeField] private BaseUserDialoguePinDrawer dialoguePopup;
        public BaseUserQuizPinDrawer QuizPopup { get => quizPopup; set => quizPopup = value; }
        [SerializeField] private BaseUserQuizPinDrawer quizPopup;
        public SpriteDrawer ImagePopup { get => imagePopup; set => imagePopup = value; }
        [SerializeField] private SpriteDrawer imagePopup;
        public BaseUserPinGroupDrawer PinButtonsPrefab { get => pinButtonsPrefab; set => pinButtonsPrefab = value; }
        [SerializeField] private BaseUserPinGroupDrawer pinButtonsPrefab;
        public BaseConfirmationPopup ConfirmationPopup { get => confirmationPopup; set => confirmationPopup = value; }
        [SerializeField] private BaseConfirmationPopup confirmationPopup;
        public AndroidBackButton BackButton { get => backButton; set => backButton = value; }
        [SerializeField] private AndroidBackButton backButton;
        public SwipeManager SwipeManager { get => swipeManager; set => swipeManager = value; }
        [SerializeField] private SwipeManager swipeManager;

        public override void InstallBindings()
        {
            Container.BindInstance(Tooltip);
            Container.BindInstance<ISidebarController>(SidebarController);
            Container.BindInstance(InstructionsPopup);
            Container.BindInstance(SettingsPopup);
            Container.BindInstance(EncounterInfoPopup);
            Container.BindInstance(DialoguePopup);
            Container.BindInstance(QuizPopup);
            Container.BindInstance(PinButtonsPrefab);
            Container.BindInstance(ImagePopup);
            Container.BindInstance(ConfirmationPopup);
            Container.BindInstance(BackButton);
            Container.BindInstance(SwipeManager);

            Container.Bind<IUserEncounterMenuSceneStarter>().To<UserEncounterMenuSceneStarter>().AsTransient();

            Container.Bind<IStatusWriter>().To<LocalStatusWriter>().AsTransient();

            Container.Bind<FeedbackColorManager>().To<FeedbackColorManager>().AsTransient();
            Container.Bind<IStringDeserializer<Color>>().To<ColorDeserializer>().AsTransient();

            Container.Bind<IStringSerializer<EncounterContentStatus>>().To<EncounterContentStatusSerializer>().AsTransient();
            Container.Bind<IStatusSerializer<SectionStatus>>().To<SectionStatusSerializer>().AsTransient();
            Container.Bind<IStatusSerializer<TabStatus>>().To<TabStatusSerializer>().AsTransient();
            Container.Bind<IStatusSerializer<PanelStatus>>().To<PanelStatusSerializer>().AsTransient();
            Container.Bind<IStatusSerializer<PinGroupStatus>>().To<PinGroupStatusSerializer>().AsTransient();
            Container.Bind<IStatusSerializer<DialogueStatus>>().To<DialogueStatusSerializer>().AsTransient();
            Container.Bind<IStatusSerializer<QuizStatus>>().To<QuizStatusSerializer>().AsTransient();

            Container.Bind<IStringDeserializer<EncounterContentStatus>>().To<EncounterContentStatusDeserializer>().AsTransient();
            Container.Bind<ICharEnumeratorDeserializer<SectionStatus>>().To<SectionStatusDeserializer>().AsTransient();
            Container.Bind<ICharEnumeratorDeserializer<TabStatus>>().To<TabStatusDeserializer>().AsTransient();
            Container.Bind<ICharEnumeratorDeserializer<string>>().To<KeyDeserializer>().AsTransient();
        }
    }
}
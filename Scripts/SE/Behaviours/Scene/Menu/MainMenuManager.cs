#if DEEP_LINKING
using ImaginationOverflow.UniversalDeepLinking;
#endif
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class MainMenuManager : SceneManager, IMenuSceneDrawer
    {
        public BaseMenuSceneDrawer MenuDrawer { get => menuDrawer; set => menuDrawer = value; }
        [SerializeField] private BaseMenuSceneDrawer menuDrawer;
        public LoadingScreen LoadingScreen { get => loadingScreen; set => loadingScreen = value; }
        [SerializeField] private LoadingScreen loadingScreen;
        public BaseInitialLoginHandler LoginHandler { get => login; set => login = value; }
        [SerializeField] private BaseInitialLoginHandler login;
        public List<GameObject> WelcomeScreens { get => welcomeScreens; }
        [SerializeField] private List<GameObject> welcomeScreens = new List<GameObject>();

        protected virtual ISelector<LoadingMenuSceneInfoSelectedEventArgs> SceneInfoSelectedListener { get; set; }
        protected IMenuEncountersInfoReader MenuInfoReader { get; set; }
        protected IEncounterQuickStarter EncounterQuickStarter { get; set; }
        protected QuickActionFactory LinkActionFactory { get; set; }
        [Inject]
        public virtual void Inject(
            ISelector<LoadingMenuSceneInfoSelectedEventArgs> sceneInfoSelectedListener,
            IMenuEncountersInfoReader menuInfoReader,
            IEncounterQuickStarter encounterQuickStarter,
            QuickActionFactory linkActionFactory)
        {
            SceneInfoSelectedListener = sceneInfoSelectedListener;
            MenuInfoReader = menuInfoReader;
            EncounterQuickStarter = encounterQuickStarter;
            LinkActionFactory = linkActionFactory;
        }

        protected override void Awake()
        {
            base.Awake();

            if (MenuDrawer is ILogoutHandler logoutHandler)
                logoutHandler.Logout += Logout;
        }
        private bool started;
        protected override void Start()
        {
            base.Start();
            started = true;

            if (SceneInfo != null) {
                MenuDrawer.Display(SceneInfo);
                SceneInfoSelectedListener.Select(this, new LoadingMenuSceneInfoSelectedEventArgs(SceneInfo));
            }
        }

        protected override void StartAsInitialScene()
        {
            if (LoginHandler != null)
                ShowInitialLogin();
            else
                //Login(User.Guest);
                Login(new User(25));
        }

        protected override void StartAsLaterScene()
        {
            Destroy(LoadingScreen.gameObject);
        }

        protected LoadingMenuSceneInfo SceneInfo { get; set; }
        public void Display(LoadingMenuSceneInfo sceneInfo)
        {
            if (sceneInfo.MenuArea == MenuArea.Cases)
                foreach (var welcomeScreen in WelcomeScreens)
                    welcomeScreen.SetActive(false);

            SceneInfo = sceneInfo;
#if DEEP_LINKING
            DeepLinkManager.Instance.LinkActivated += Instance_LinkActivated;
#endif
            sceneInfo.Result.AddOnCompletedListener(SceneInfoLoaded);

            if (started) {
                MenuDrawer.Display(sceneInfo);
                SceneInfoSelectedListener.Select(this, new LoadingMenuSceneInfoSelectedEventArgs(SceneInfo));
            }
        }
        protected virtual void SceneInfoLoaded(TaskResult<MenuSceneInfo> sceneInfo)
        {
            if (!sceneInfo.HasValue())
                return;
            if (sceneInfo.Value.LoadingScreen != null)
                sceneInfo.Value.LoadingScreen.Stop();
        }

        protected virtual void Logout()
        {
            var user = LoginHandler.Login();
            user.AddOnCompletedListener(Login);
        }
        protected virtual void ShowInitialLogin()
        {
            var user = LoginHandler.InitialLogin(LoadingScreen);
            user.AddOnCompletedListener(Login);
        }

        protected virtual void Login(TaskResult<User> user) => Login(user.Value);
        protected virtual void Login(User user)
        {
            var menuEncounters = MenuInfoReader.GetMenuEncountersInfo(user);
            var menuSceneInfo = new LoadingMenuSceneInfo(user, LoadingScreen, MenuArea.InitialSelection, menuEncounters);
            if (started)
                Display(menuSceneInfo);
            else
                SceneInfo = menuSceneInfo;
        }

#if DEEP_LINKING
        protected virtual void OnDestroy() => DeepLinkManager.Instance.LinkActivated -= Instance_LinkActivated;

        public void TestLink()
        {
            var dictionary = new Dictionary<string, string>();
            dictionary.Add("id", "73");
            var linkActivation = new LinkActivation("lift://encounter?id=73", "id=73", dictionary);

            Instance_LinkActivated(linkActivation);
        }

        protected virtual void Instance_LinkActivated(LinkActivation s)
        {
            if (Instance != this)
                return;

            QuickAction quickAction = LinkActionFactory.GetLinkAction(s);
            if (quickAction.Action == QuickActionType.NA)
                return;

            SceneInfo.Result.RemoveListeners();
            EncounterQuickStarter.StartEncounter(SceneInfo.User, SceneInfo.LoadingScreen, SceneInfo.MenuEncountersInfo, quickAction.EncounterId);
        }
#endif
    }
}
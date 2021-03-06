﻿#if DEEP_LINKING
using ImaginationOverflow.UniversalDeepLinking;
#endif
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class MainMenuManager : SceneManager<LoadingMenuSceneInfo>, IMenuSceneDrawer
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

#if DEEP_LINKING
            DeepLinkManager.Instance.LinkActivated += Instance_LinkActivated;
#endif
        }

        protected override void StartAsInitialScene()
        {
            if (LoginHandler != null)
                ShowInitialLogin();
            else
                Login(User.Guest);
        }

        protected override void StartAsLaterScene() => Destroy(LoadingScreen.gameObject);

        protected override void ProcessSceneInfo(LoadingMenuSceneInfo sceneInfo)
        {
#if DEEP_LINKING
            if (onLoadAction != null && onLoadAction.Action == QuickActionType.Reader) {
                SceneInfo.Result.RemoveListeners();
                EncounterQuickStarter.StartEncounter(SceneInfo.User, SceneInfo.LoadingScreen, SceneInfo.MenuEncountersInfo, onLoadAction.EncounterId);
                return;
            }
#endif

            if (sceneInfo.MenuArea == MenuArea.Cases)
                foreach (var welcomeScreen in WelcomeScreens)
                    welcomeScreen.SetActive(false);

            sceneInfo.Result.AddOnCompletedListener(SceneInfoLoaded);

            MenuDrawer.Display(sceneInfo);
            SceneInfoSelectedListener.Select(this, new LoadingMenuSceneInfoSelectedEventArgs(sceneInfo));
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
            Display(menuSceneInfo);
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

        private QuickAction onLoadAction;
        protected virtual void Instance_LinkActivated(LinkActivation s)
        {
            if (Instance != this)
                return;

            QuickAction quickAction = LinkActionFactory.GetLinkAction(s);
            if (quickAction.Action == QuickActionType.NA)
                return;

            if (SceneInfo != null) {
                SceneInfo.Result.RemoveListeners();
                EncounterQuickStarter.StartEncounter(SceneInfo.User, SceneInfo.LoadingScreen, SceneInfo.MenuEncountersInfo, quickAction.EncounterId);
            } else {
                onLoadAction = quickAction;
            }
        }
#endif
    }
}
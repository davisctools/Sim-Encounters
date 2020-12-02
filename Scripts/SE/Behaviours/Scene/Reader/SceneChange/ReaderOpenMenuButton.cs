using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ReaderOpenMenuButton : MonoBehaviour
    {
        public Button Button { get => button; set => button = value; }
        [SerializeField] private Button button;
        public bool ShowConfirmation { get => showConfirmation; set => showConfirmation = value; }
        [SerializeField] private bool showConfirmation = true;

        protected IUserEncounterMenuSceneStarter MenuSceneStarter { get; set; }

        protected ISelectedListener<LoadingReaderSceneInfoSelectedEventArgs> LoadingReaderSceneInfoSelector { get; set; }
        [Inject]
        public virtual void Inject(
            IUserEncounterMenuSceneStarter menuSceneStarter,
            ISelectedListener<LoadingReaderSceneInfoSelectedEventArgs> loadingReaderSceneInfoTabSelector)
        {
            LoadingReaderSceneInfoSelector = loadingReaderSceneInfoTabSelector;
            MenuSceneStarter = menuSceneStarter;
        }
        protected virtual void Start()
        {
            LoadingReaderSceneInfoSelector.Selected += OnLoadingReaderSceneInfoSelected;
            if (LoadingReaderSceneInfoSelector.CurrentValue != null)
                OnLoadingReaderSceneInfoSelected(LoadingReaderSceneInfoSelector, LoadingReaderSceneInfoSelector.CurrentValue);
        }
        protected virtual void Awake() => Button.onClick.AddListener(OpenMenu);
        protected virtual void OnDestroy() 
            => LoadingReaderSceneInfoSelector.Selected -= OnLoadingReaderSceneInfoSelected;

        protected LoadingReaderSceneInfo SceneInfo { get; set; }
        protected virtual void OnLoadingReaderSceneInfoSelected(object sender, LoadingReaderSceneInfoSelectedEventArgs eventArgs)
            => SceneInfo = eventArgs.SceneInfo;

        protected virtual void OpenMenu()
        {
            var loadingScreen = SceneInfo.LoadingScreen;
            if (!SceneInfo.Result.IsCompleted() || !SceneInfo.Result.Result.HasValue()) {
                var user = SceneInfo.User;
                if (ShowConfirmation)
                    MenuSceneStarter.ConfirmStartingMenuScene(user, loadingScreen);
                else
                    MenuSceneStarter.StartMenuScene(user, loadingScreen);
            } else {
                var encounter = SceneInfo.Result.Result.Value.Encounter;
                if (ShowConfirmation)
                    MenuSceneStarter.ConfirmStartingMenuScene(encounter, loadingScreen);
                else
                    MenuSceneStarter.StartMenuScene(encounter, loadingScreen);
            }
        }
    }
}
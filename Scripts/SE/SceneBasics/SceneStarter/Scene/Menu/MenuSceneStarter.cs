using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class MenuSceneStarter : IMenuSceneStarter
    {
        protected string ScenePath { get; }
        protected SignalBus SignalBus { get; }
        public MenuSceneStarter(string scenePath, SignalBus signalBus)
        {
            ScenePath = scenePath;
            SignalBus = signalBus;
        }

        public virtual void StartScene(LoadingMenuSceneInfo data)
        {
            data.LoadingScreen?.Show();
            SignalBus.Fire<SceneChangedSignal>();

            var loading = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(ScenePath);
            loading.completed += (asyncOperation) => InitializeScene(data);
        }

        protected virtual void InitializeScene(LoadingMenuSceneInfo data)
        {
            if (SceneManager<LoadingMenuSceneInfo>.Instance == null) {
                Debug.LogError("Started scene UI is not Menu scene.");
                return;
            }

            SceneManager<LoadingMenuSceneInfo>.Instance.Display(data);
        }
    }
}

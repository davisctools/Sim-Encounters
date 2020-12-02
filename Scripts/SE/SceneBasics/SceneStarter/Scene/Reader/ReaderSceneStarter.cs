using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ReaderSceneStarter : IReaderSceneStarter
    {
        protected string ScenePath { get; }
        protected SignalBus SignalBus { get; }
        public ReaderSceneStarter(string scenePath, SignalBus signalBus)
        {
            ScenePath = scenePath;
            SignalBus = signalBus;
        }

        public virtual void StartScene(LoadingReaderSceneInfo data)
        {
            data.LoadingScreen?.Show();
            SignalBus.Fire<SceneChangedSignal>();

            var loading = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(ScenePath);
            loading.completed += (asyncOperation) => InitializeScene(data);
        }

        protected virtual void InitializeScene(LoadingReaderSceneInfo data)
        {
            if (!(SceneManager.Instance is IReaderSceneDrawer readerScene)) {
                Debug.LogError("Started scene UI is not Reader.");
                return;
            }

            readerScene.Display(data);
        }
    }
}
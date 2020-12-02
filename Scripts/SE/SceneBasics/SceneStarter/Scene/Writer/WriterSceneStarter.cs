using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class WriterSceneStarter : IWriterSceneStarter
    {
        protected string ScenePath { get; }
        protected SignalBus SignalBus { get; }
        public WriterSceneStarter(string scenePath, SignalBus signalBus)
        {
            ScenePath = scenePath;
            SignalBus = signalBus;
        }

        public virtual void StartScene(LoadingWriterSceneInfo data)
        {
            data.LoadingScreen?.Show();
            SignalBus.Fire<SceneChangedSignal>();

            var loading = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(ScenePath);
            loading.completed += (asyncOperation) => InitializeScene(data);
        }

        protected virtual void InitializeScene(LoadingWriterSceneInfo data)
        {
            if (!(SceneManager.Instance is ILoadingWriterSceneDrawer writerScene)) {
                Debug.LogError("Started scene UI is not Reader.");
                return;
            }

            writerScene.Display(data);
        }
    }
}
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    [RequireComponent(typeof(Button))]
    public class WriterSaveAndViewInReaderButton : MonoBehaviour
    {
        protected SignalBus SignalBus { get; set; }
        protected ISelector<WriterSceneInfoSelectedEventArgs> SceneInfoSelector { get; set; }
        protected IReaderSceneStarter ReaderSceneStarter { get; set; }
        protected IEncounterWriter EncounterWriter { get; set; }
        [Inject]
        public virtual void Inject(
            SignalBus signalBus,
            ISelector<WriterSceneInfoSelectedEventArgs> sceneInfoSelector,
            IReaderSceneStarter sceneStarter,
            [Inject(Id = SaveType.Local)] IEncounterWriter encounterWriter)
        {
            SignalBus = signalBus;
            SceneInfoSelector = sceneInfoSelector;
            ReaderSceneStarter = sceneStarter;
            EncounterWriter = encounterWriter;
        }

        protected virtual Button Button { get; set; }
        protected virtual void Start()
        {
            Button = GetComponent<Button>();
            Button.interactable = false;

            SceneInfoSelector.Selected += SceneLoaded;
            if (SceneInfoSelector.CurrentValue != null)
                SceneLoaded(this, SceneInfoSelector.CurrentValue);
        }

        private void SceneLoaded(object sender, WriterSceneInfoSelectedEventArgs e)
        {
            Button.interactable = true;
            Button.onClick.AddListener(ShowInReader);
        }

        protected virtual void ShowInReader()
        {
            SignalBus.Fire<SceneChangedSignal>();
            var sceneInfo = SceneInfoSelector.CurrentValue.SceneInfo;
            EncounterWriter.Save(sceneInfo.User, sceneInfo.Encounter);

            var encounterStatus = new EncounterStatus(new EncounterBasicStatus(), new EncounterContentStatus());
            var encounter = new UserEncounter(sceneInfo.User, sceneInfo.Encounter, encounterStatus);
            var encounterResult = new WaitableTask<UserEncounter>(encounter);
            var loadingInfo = new LoadingReaderSceneInfo(sceneInfo.User, sceneInfo.LoadingScreen, encounterResult);
            ReaderSceneStarter.StartScene(loadingInfo);
        }
    }
}
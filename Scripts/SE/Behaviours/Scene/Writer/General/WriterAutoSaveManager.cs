using ClinicalTools.UI;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class WriterAutoSaveManager : MonoBehaviour
    {
        protected SignalBus SignalBus { get; set; }
        protected BaseMessageHandler MessageHandler { get; set; }
        protected ISelector<WriterSceneInfoSelectedEventArgs> SceneInfoSelector { get; set; }
        protected IEncounterWriter EncounterServerWriter { get; set; }
        protected IEncounterWriter EncounterAutosaveWriter { get; set; }
        [Inject]
        public virtual void Inject(
            SignalBus signalBus,
            BaseMessageHandler messageHandler,
            ISelector<WriterSceneInfoSelectedEventArgs> sceneInfoSelector,
            [Inject(Id = SaveType.Server)] IEncounterWriter encounterServerWriter,
            [Inject(Id = SaveType.Local)] IEncounterWriter encounterAutosaveWriter)
        {
            SignalBus = signalBus;
            MessageHandler = messageHandler;
            SceneInfoSelector = sceneInfoSelector;
            EncounterServerWriter = encounterServerWriter;
            EncounterAutosaveWriter = encounterAutosaveWriter;
        }

        protected virtual Button Button { get; set; }
        protected virtual void Start()
        {
            SceneInfoSelector.Selected += SceneLoaded;
            if (SceneInfoSelector.CurrentValue != null)
                SceneLoaded(this, SceneInfoSelector.CurrentValue);
        }

        protected Coroutine CurrentCoroutine { get; set; }
        private void SceneLoaded(object sender, WriterSceneInfoSelectedEventArgs e)
        {
            if (CurrentCoroutine != null)
                StopCoroutine(CurrentCoroutine);
            CurrentCoroutine = StartCoroutine(AutosaveCoroutine());
        }

        private const float AutosaveIntervalSeconds = 3 * 60; // In seconds
        protected virtual IEnumerator AutosaveCoroutine()
        {
            yield return new WaitForSeconds(AutosaveIntervalSeconds);
            //AutosaveEncounter();

            yield return AutosaveCoroutine();
        }

        protected WriterSceneInfo SceneInfo => SceneInfoSelector.CurrentValue.SceneInfo;
        protected virtual void AutosaveEncounter()
        {
            SignalBus.Fire<SerializeEncounterSignal>();
            var sceneInfo = SceneInfoSelector.CurrentValue.SceneInfo;

            var saveParameters = new SaveEncounterParameters() {
                Encounter = sceneInfo.Encounter,
                User = sceneInfo.User,
                SaveVersion = SaveVersion.AutoSave
            };
            var writerTask = EncounterServerWriter.Save(saveParameters);
            writerTask.AddOnCompletedListener((result) => AutosaveCompleted(result, saveParameters));
        }

        protected virtual void AutosaveCompleted(TaskResult result, SaveEncounterParameters saveParameters)
        {
            if (!result.IsError()) { 
                MessageHandler.ShowMessage("Encounter autosaved.");
            } else {
                MessageHandler.ShowMessage($"Error autosaving encounter. Creating local save.\n{result.Exception.Message}", MessageType.Error);
                var writerTask = EncounterAutosaveWriter.Save(saveParameters);
            }
        }
    }
}
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
        protected ISelectedListener<WriterSceneInfoSelectedEventArgs> SceneInfoSelectedListener { get; set; }
        protected IEncounterWriter EncounterWriter { get; set; }
        [Inject]
        public virtual void Inject(
            SignalBus signalBus,
            BaseMessageHandler messageHandler,
            ISelectedListener<WriterSceneInfoSelectedEventArgs> sceneInfoSelectedListener,
            [Inject(Id = SaveType.Autosave)] IEncounterWriter encounterWriter)
        {
            SignalBus = signalBus;
            MessageHandler = messageHandler;
            SceneInfoSelectedListener = sceneInfoSelectedListener;
            EncounterWriter = encounterWriter;
        }

        protected virtual Button Button { get; set; }
        protected virtual void Start()
        {
            SceneInfoSelectedListener.Selected += SceneLoaded;
            if (SceneInfoSelectedListener.CurrentValue != null)
                SceneLoaded(this, SceneInfoSelectedListener.CurrentValue);
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
            AutosaveEncounter();

            yield return AutosaveCoroutine();
        }

        protected WriterSceneInfo SceneInfo => SceneInfoSelectedListener.CurrentValue.SceneInfo;
        protected virtual void AutosaveEncounter()
        {
            SignalBus.Fire<SerializeEncounterSignal>();
            var sceneInfo = SceneInfoSelectedListener.CurrentValue.SceneInfo;
            var writerTask = EncounterWriter.Save(sceneInfo.User, sceneInfo.Encounter);
            writerTask.AddOnCompletedListener(AutosaveCompleted);
        }

        protected virtual void AutosaveCompleted(TaskResult result)
        {
            if (!result.IsError())
                MessageHandler.ShowMessage("Encounter autosaved.");
        }
    }
}
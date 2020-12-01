using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class WriterAutoSaveManager : MonoBehaviour
    {
        protected SignalBus SignalBus { get; set; }
        protected ISelector<WriterSceneInfoSelectedEventArgs> SceneInfoSelector { get; set; }
        protected IEncounterWriter EncounterWriter { get; set; }
        [Inject]
        public virtual void Inject(
            SignalBus signalBus,
            ISelector<WriterSceneInfoSelectedEventArgs> sceneInfoSelector,
            [Inject(Id = SaveType.Autosave)] IEncounterWriter encounterWriter)
        {
            SignalBus = signalBus;
            SceneInfoSelector = sceneInfoSelector;
            EncounterWriter = encounterWriter;
        }

        protected virtual Button Button { get; set; }
        protected virtual void Start()
        {
            SceneInfoSelector.Selected += SceneLoaded;
            if (SceneInfoSelector.CurrentValue != null)
                SceneLoaded(this, SceneInfoSelector.CurrentValue);
        }

        private void SceneLoaded(object sender, WriterSceneInfoSelectedEventArgs e)
            => StartCoroutine(AutosaveCoroutine());

        private const float AutosaveIntervalSeconds = 3 * 60; // In seconds
        protected virtual IEnumerator AutosaveCoroutine()
        {
            yield return new WaitForSeconds(AutosaveIntervalSeconds);
            AutosaveEncounter();

            yield return AutosaveCoroutine();
        }

        protected WriterSceneInfo SceneInfo => SceneInfoSelector.CurrentValue.SceneInfo;
        protected virtual void AutosaveEncounter()
        {
            SignalBus.Fire<SerializeEncounterSignal>();
            var sceneInfo = SceneInfoSelector.CurrentValue.SceneInfo;
            EncounterWriter.Save(sceneInfo.User, sceneInfo.Encounter);
        }
    }
}
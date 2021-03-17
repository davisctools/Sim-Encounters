using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class EncounterLockRefresher : MonoBehaviour
    {
        protected SignalBus SignalBus { get; set; }
        protected IEncounterLocksReader EncounterLocksReader { get; set; }
        protected ISelectedListener<MenuSceneInfoSelectedEventArgs> SceneSelectedListener { get; set; }

        [Inject]
        public virtual void Inject(
             SignalBus signalBus,
             IEncounterLocksReader encounterLocksReader,
             ISelectedListener<MenuSceneInfoSelectedEventArgs> sceneSelectedListener)
        {
            SignalBus = signalBus;
            EncounterLocksReader = encounterLocksReader;
            SceneSelectedListener = sceneSelectedListener;
        }


        public virtual void Start() => StartCoroutine(RefreshLocksRoutine());

        private const float RefreshIntervalSeconds = 10;
        protected virtual IEnumerator RefreshLocksRoutine()
        {
            yield return null;
            RefreshLocks();
            yield return new WaitForSeconds(RefreshIntervalSeconds);

            yield return RefreshLocksRoutine();
        }

        protected virtual void RefreshLocks()
        {
            if (SceneSelectedListener?.CurrentValue?.SceneInfo == null)
                return;

            var sceneInfo = SceneSelectedListener.CurrentValue.SceneInfo;
            var task = EncounterLocksReader.GetEncounterLocks(sceneInfo.User);
            task.AddOnCompletedListener(RetrievedLocks);
        }

        protected virtual void RetrievedLocks(TaskResult<Dictionary<int, EncounterEditLock>> result)
        {
            if (!result.HasValue())
                return;

            var locks = result.Value;
            var sceneInfo = SceneSelectedListener.CurrentValue.SceneInfo;
            foreach (var encounter in sceneInfo.MenuEncountersInfo.GetEncounters()) {
                var recordNumber = encounter.GetLatestMetadata().RecordNumber;
                encounter.Lock = locks.ContainsKey(recordNumber) ? locks[recordNumber] : null;
            }
            SignalBus.Fire<EncounterLocksUpdatedSignal>();
        }
    }
}

using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class WriterEncounterLockManager : MonoBehaviour
    {
        protected IEncounterLocker EncounterLocker { get; set; }
        protected IEncounterUnlocker EncounterUnlocker { get; set; }
        protected ISelectedListener<WriterSceneInfoSelectedEventArgs> SceneSelectedListener { get; set; }
        
       [Inject] public virtual void Inject(
            IEncounterLocker encounterLocker, 
            IEncounterUnlocker encounterUnlocker,
            ISelectedListener<WriterSceneInfoSelectedEventArgs> sceneSelectedListener)
        {
            EncounterLocker = encounterLocker;
            EncounterUnlocker = encounterUnlocker;
            SceneSelectedListener = sceneSelectedListener;
        }

        protected virtual Coroutine LockRefreshRoutine { get; set; }
        public virtual void Start() => LockRefreshRoutine = StartCoroutine(LockRefresh());

        private const float LockIntervalSeconds = 30;
        protected virtual IEnumerator LockRefresh()
        {
            yield return new WaitForSeconds(LockIntervalSeconds);
            var sceneInfo = SceneSelectedListener.CurrentValue.SceneInfo;
            var task = EncounterLocker.LockEncounter(sceneInfo.User, sceneInfo.Encounter.Metadata);
            task.AddOnCompletedListener(EncounterRelocked);

            yield return LockRefresh();
        }

        protected virtual void OnDestroy()
        {
            if (LockRefreshRoutine != null)
                StopCoroutine(LockRefreshRoutine);

            var sceneInfo = SceneSelectedListener.CurrentValue.SceneInfo;
            var task = EncounterUnlocker.UnlockEncounter(sceneInfo.User, sceneInfo.Encounter.Metadata);
            task.AddOnCompletedListener(EncounterUnlocked);
        }

        protected virtual void EncounterRelocked(TaskResult result) { }
        protected virtual void EncounterUnlocked(TaskResult result) { }
    }
}

using System.Collections;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class EncounterLockRefresher : MonoBehaviour
    {
        protected IEncounterLocker EncounterLocker { get; set; }
        protected IEncounterUnlocker EncounterUnlocker { get; set; }
        protected ISelectedListener<WriterSceneInfoSelectedEventArgs> SceneSelectedListener { get; set; }
        
        public virtual void Inject(
            IEncounterLocker encounterLocker, 
            IEncounterUnlocker encounterUnlocker,
            ISelectedListener<WriterSceneInfoSelectedEventArgs> sceneSelectedListener)
        {
            EncounterLocker = encounterLocker;
            EncounterUnlocker = encounterUnlocker;
            SceneSelectedListener = sceneSelectedListener;
        }


        public virtual void Start() => StartCoroutine(LockRefresh());

        private const float LockIntervalSeconds = 3 * 60;
        protected virtual IEnumerator LockRefresh()
        {
            yield return new WaitForSeconds(LockIntervalSeconds);
            var sceneInfo = SceneSelectedListener.CurrentValue.SceneInfo;
            EncounterLocker.LockEncounter(sceneInfo.User, sceneInfo.Encounter.Metadata);

            yield return LockRefresh();
        }

        protected virtual void OnDestroy()
        {
            var sceneInfo = SceneSelectedListener.CurrentValue.SceneInfo;
            EncounterUnlocker.UnlockEncounter(sceneInfo.User, sceneInfo.Encounter.Metadata);
        }
    }
}

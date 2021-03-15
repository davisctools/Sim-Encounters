using System.Collections;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public interface IEncounterLocker
    {
        WaitableTask LockEncounter(User user, EncounterMetadata metadata);
        WaitableTask RefreshEncounterLock(User user, EncounterMetadata metadata);
        WaitableTask UnlockEncounter(User user, EncounterMetadata metadata);
    }

    public class EncounterEditLock
    {
        public string EditorName { get; set; }
        public long StartEditTime { get; set; }
    }

    public class EncounterLockRefresher : MonoBehaviour
    {
        protected IEncounterLocker EncounterLocker { get; set; }
            protected ISelectedListener<WriterSceneInfoSelectedEventArgs> SceneSelectedListener { get; set; }
        public virtual void Inject(IEncounterLocker encounterLocker,
            ISelectedListener<WriterSceneInfoSelectedEventArgs> sceneSelectedListener)
        {
            EncounterLocker = encounterLocker;
            SceneSelectedListener = sceneSelectedListener;
        }


        public virtual void Start()
        {
            StartCoroutine(LockRefresh());
        }

        private const float LockIntervalSeconds = 3 * 60; // In seconds
        protected virtual IEnumerator LockRefresh()
        {
            yield return new WaitForSeconds(LockIntervalSeconds);
            var sceneInfo = SceneSelectedListener.CurrentValue.SceneInfo;
            EncounterLocker.LockEncounter(sceneInfo.User, sceneInfo.Encounter.Metadata);

            yield return LockRefresh();
        }
    }


    public class EncounterLock : IEncounterLocker
    {
        public WaitableTask LockEncounter(User user, EncounterMetadata metadata)
        {
            Debug.Log("Encounter Locked");
            return new WaitableTask(true);
        }

        public WaitableTask RefreshEncounterLock(User user, EncounterMetadata metadata)
        {
            Debug.Log("Encounter Lock Refreshed");
            return new WaitableTask(true);
        }

        public WaitableTask UnlockEncounter(User user, EncounterMetadata metadata)
        {
            Debug.Log("Encounter Unlocked");
            return new WaitableTask(true);
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

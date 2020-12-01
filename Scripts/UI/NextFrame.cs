using System.Collections;
using UnityEngine;
using System;

namespace ClinicalTools.UI
{
    public class NextFrame : MonoBehaviour
    {
        protected static NextFrame Instance { get; set; }

        void Awake() 
        {
            if (Instance != null) {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);
            Instance = this;
        }

        /// <summary>
        /// Calls the specified function at the very end of the current frame.
        /// </summary>
        /// <param name="action"> Type the function you want to call. Does not support functions that require arguements.</param>
        public static void Function(Action action)
        {
            if (action != null && Instance)
                Instance.StartCoroutine(WaitforNext(action));
        }

        static IEnumerator WaitforNext(Action action)
        {
            yield return new WaitForEndOfFrame();
            action();
        }
    }
}
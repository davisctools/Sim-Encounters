using System;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public abstract class SceneManager : MonoBehaviour
    {
        protected static bool FirstScene { get; set; } = true;
        public static SceneManager Instance { get; protected set; }

        protected virtual void Awake()
        {
            Instance = this;
        }
        protected virtual void Start()
        {
            if (FirstScene)
                StartAsInitialScene();
            else
                StartAsLaterScene();

            FirstScene = false;
        }

        protected abstract void StartAsInitialScene();
        protected abstract void StartAsLaterScene();
    }
}
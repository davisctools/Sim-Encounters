using System;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public abstract class SceneManager<T> : SceneManager
    {
        // This is messy and reinforces a one instance per scene, which contradicts embedded scenes.

        // Perhaps the actual scene info should be passed through dependency injection, similar to
        // how encounter information is.
        // The main issue is that I would need to ensure that is wiped before the next scene change.
        // That's not necessarily a bad thing, but it does require a bit more maintenance.
        public new static SceneManager<T> Instance { get; protected set; }

        public virtual void Display(T sceneInfo)
        {
            // Ensure that it's the main scene manager
            if (SceneManager.Instance == this)
                ImageHolder.StopHoldingData();
            ProcessSceneInfo(sceneInfo);
        }
        protected abstract void ProcessSceneInfo(T sceneInfo);
    }

    public abstract class SceneManager : MonoBehaviour
    {
        protected static bool FirstScene { get; set; } = true;
        public static SceneManager Instance { get; protected set; }

        protected virtual void Awake()
        {
            // The main scene manager from a scene is expected to not have a parent
            // This allows embedded scenes within scenes, while allowing them to
            // avoid causing overarching scene changes
            if (transform.parent != null)
                Instance = this;
        }
        protected virtual void Start()
        {
            if (FirstScene)
                StartAsInitialScene();
            else
                StartAsLaterScene();

            FirstScene = false;

            ImageHolder.StopHoldingData();
        }

        protected abstract void StartAsInitialScene();
        protected abstract void StartAsLaterScene();
    }
}
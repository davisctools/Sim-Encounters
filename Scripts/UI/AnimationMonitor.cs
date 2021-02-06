using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.UI
{
    /// <summary>
    /// Helps monitor when animations are in progress to allow waiting to perform expensive tasks when animations end.
    /// </summary>
    /// <remarks>
    /// Making this a MonoBehaviour helps prevent it from staying between scenes, where it's easier for animations to
    /// mistakenly not be marked as stopped. 
    /// This also allows Coroutines to be called directly off of this class, which helps prevent calling coroutines on
    /// inactive objects.
    /// </remarks>
    public class AnimationMonitor : MonoBehaviour
    {
        protected virtual HashSet<MonoBehaviour> AnimatedObjects { get; } = new HashSet<MonoBehaviour>();

        public virtual void AnimationStarting(MonoBehaviour behaviour)
        {
            if (!AnimatedObjects.Contains(behaviour))
                AnimatedObjects.Add(behaviour);
        }
        public virtual void AnimationStopping(MonoBehaviour behaviour)
        {
            if (AnimatedObjects.Contains(behaviour))
                AnimatedObjects.Remove(behaviour);
        }

        public virtual bool IsAnimationInProgress() => AnimatedObjects.Count > 0;
    }
}
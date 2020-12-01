using ClinicalTools.UI;
using UnityEngine;
using Zenject;

namespace ClinicalTools.UI
{
    public class DisableSwipeWhileOpen : MonoBehaviour
    {
        protected SwipeManager SwipeManager { get; set; }
        [Inject]
        public virtual void Inject(SwipeManager swipeManager) => SwipeManager = swipeManager;

        protected virtual void OnEnable()
        {
            if (SwipeManager != null)
                SwipeManager.DisableSwipe();
        }
        protected virtual void OnDisable()
        {
            if (SwipeManager != null)
                SwipeManager.ReenableSwipe();
        }
    }
}
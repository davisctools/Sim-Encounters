using UnityEngine;
using Zenject;

namespace ClinicalTools.UI
{
    public class DisableOnAndroidBackButton : MonoBehaviour
    {
        protected AndroidBackButton BackButton { get; set; }

        [Inject]
        public virtual void Inject(AndroidBackButton backButton)
            => BackButton = backButton;

        protected virtual void OnEnable()
        {
            if (BackButton != null)
                BackButton.Register(Disable);
        }

        protected virtual void OnDisable()
        {
            if (BackButton != null)
                BackButton.Deregister(Disable);
        }

        protected virtual void Disable() => gameObject.SetActive(false);
    }
}

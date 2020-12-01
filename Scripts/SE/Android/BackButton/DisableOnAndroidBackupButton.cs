using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class DisableOnAndroidBackupButton : MonoBehaviour
    {
        protected AndroidBackButton BackButton { get; set; }

        [Inject]
        public virtual void Inject(AndroidBackButton backButton)
            => BackButton = backButton;

        protected virtual void OnEnable() => BackButton.Register(Disable);
        protected virtual void OnDisable() => BackButton.Deregister(Disable);
        protected virtual void Disable() => gameObject.SetActive(false);
    }
}

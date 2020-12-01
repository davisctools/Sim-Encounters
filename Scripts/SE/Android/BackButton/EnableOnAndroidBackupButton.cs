using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class EnableOnAndroidBackupButton : MonoBehaviour
    {
        protected AndroidBackButton BackButton { get; set; }

        [Inject]
        public virtual void Inject(AndroidBackButton backButton)
            => BackButton = backButton;

        protected virtual void OnEnable() => BackButton.Deregister(Enable);
        protected virtual void OnDisable() => BackButton.Register(Enable);
        protected virtual void Enable() => gameObject.SetActive(true);
    }
}

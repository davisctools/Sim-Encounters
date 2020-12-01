using UnityEngine;
using UnityEngine.UI;
using Zenject;
using ClinicalTools.UI.Extensions;
using System.Collections;

namespace ClinicalTools.UI
{
    [RequireComponent(typeof(Toggle))]
    public class CloseToggle : MonoBehaviour
    {
        public int DelayFrames { get => delayFrames; set => delayFrames = value; }
        [SerializeField] private int delayFrames;

        private Toggle toggle;
        protected Toggle Toggle {
            get {
                if (toggle == null)
                    toggle = GetComponent<Toggle>();
                return toggle;
            }
        }


        protected ICloseHandler CloseHandler { get; set; }
        [Inject] public virtual void Inject(ICloseHandler closeHandler) => CloseHandler = closeHandler;

        protected virtual void Awake() => Toggle.AddOnSelectListener(Close);
        protected virtual void Close()
        {
            if (DelayFrames > 0)
                StartCoroutine(CloseCoroutine());
            else
                CloseHandler?.Close(this);
        }
        protected virtual IEnumerator CloseCoroutine()
        {
            for (int i = 0; i < DelayFrames; i++)
                yield return null;

            CloseHandler?.Close(this);
        }
    }
}
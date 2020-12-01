using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.UI
{
    [RequireComponent(typeof(Button))]
    public class CloseButton : MonoBehaviour
    {
        private Button button;
        protected Button Button
        {
            get {
                if (button == null)
                    button = GetComponent<Button>();
                return button;
            }
        }

        protected ICloseHandler CloseHandler { get; set; }
        [Inject] public virtual void Inject(ICloseHandler closeHandler) => CloseHandler = closeHandler;

        protected virtual void Awake() => Button.onClick.AddListener(Close);
        protected virtual void Close() => CloseHandler?.Close(this);
    }
}
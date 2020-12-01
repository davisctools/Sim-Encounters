using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.UI
{
    public class ResizeButton : MonoBehaviour
    {
        public bool Enlarge { get => enlarge; set => enlarge = value; }
        [SerializeField] private bool enlarge;

        private Button button;
        public Button Button {
            get {
                if (button == null)
                    button = GetComponent<Button>();
                return button;
            }
        }

        protected virtual void Awake() => Button.onClick.AddListener(Resize);

        protected virtual void Resize()
        {
            CanvasResizer.ResizeValue01 += GetResizeValue();
            Update();
        }

        private float lastResizeValue = -1f;
        protected virtual void Update()
        {
            if (lastResizeValue == CanvasResizer.ResizeValue01)
                return;

            lastResizeValue = CanvasResizer.ResizeValue01;
            Button.interactable = Enlarge ? lastResizeValue < .95f : lastResizeValue > .05f;
        }

        private const float ResizeValue = .1f;
        protected float GetResizeValue() => Enlarge ? ResizeValue : -ResizeValue;
    }
}
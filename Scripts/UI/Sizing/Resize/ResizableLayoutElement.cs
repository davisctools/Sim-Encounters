using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.UI
{
    [RequireComponent(typeof(LayoutElement))]
    public class ResizableLayoutElement : MonoBehaviour
    {
        [SerializeField] private bool resizableMinWidth;
        [SerializeField] private bool resizablePreferredWidth;
        [SerializeField] private bool resizableMinHeight;
        [SerializeField] private bool resizablePreferredHeight;

        private float defaultMinWidth, defaultPreferredWidth, defaultMinHeight, defaultPreferredHeight;
        protected LayoutElement LayoutElement { get; set; }
        protected CanvasResizer Resizer { get; set; }

        [Inject]
        public virtual void Inject(CanvasResizer resizer)
        {
            LayoutElement = GetComponent<LayoutElement>();
            defaultMinWidth = LayoutElement.minWidth;
            defaultPreferredWidth = LayoutElement.preferredWidth;
            defaultMinHeight = LayoutElement.minHeight;
            defaultPreferredHeight = LayoutElement.preferredHeight;

            Resizer = resizer;
            Resized(Resizer.ResizeValue);
        }

        // OnDestroy is only called on objects that were once enabled, so adding listener here ensures it's only added if enabled
        protected virtual void Start()
        {
            Resized(Resizer.ResizeValue);
            Resizer.Resized += Resized;
        }

        protected virtual void Resized(float size)
        {
            if (resizableMinWidth)
                LayoutElement.minWidth = defaultMinWidth * size;
            if (resizablePreferredWidth)
                LayoutElement.preferredWidth = defaultPreferredWidth * size;
            if (resizableMinHeight)
                LayoutElement.minHeight = defaultMinHeight * size;
            if (resizablePreferredHeight)
                LayoutElement.preferredHeight = defaultPreferredHeight * size;
        }

        protected virtual void OnDestroy()
        {
            if (Resizer) Resizer.Resized -= Resized;
        }
    }
}
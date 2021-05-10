using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.UI
{
    [RequireComponent(typeof(HorizontalOrVerticalLayoutGroup))]
    public class ResizableLayoutGroup : MonoBehaviour
    {
        [SerializeField] private bool resizableLeft;
        [SerializeField] private bool resizableRight;
        [SerializeField] private bool resizableTop;
        [SerializeField] private bool resizableBottom;
        [SerializeField] private bool resizableSpacing;


        private int defaultLeft, defaultRight, defaultTop, defaultBottom;
        private float defaultSpacing;
        protected HorizontalOrVerticalLayoutGroup LayoutGroup { get; set; }
        protected CanvasResizer Resizer { get; set; }
        [Inject]
        public virtual void Inject(CanvasResizer resizer)
        {
            LayoutGroup = GetComponent<HorizontalOrVerticalLayoutGroup>();
            defaultLeft = LayoutGroup.padding.left;
            defaultRight = LayoutGroup.padding.right;
            defaultTop = LayoutGroup.padding.top;
            defaultBottom = LayoutGroup.padding.bottom;
            defaultSpacing = LayoutGroup.spacing;

            Resizer = resizer;
            Resized(Resizer.ResizeValue);
        }

        protected virtual void Start()
        {
            Resized(Resizer.ResizeValue);
            Resizer.Resized += Resized;
        }

        protected virtual void Resized(float size)
        {
            if (resizableLeft)
                LayoutGroup.padding.left = (int)(defaultLeft * size);
            if (resizableRight)
                LayoutGroup.padding.right = (int)(defaultRight * size);
            if (resizableTop)
                LayoutGroup.padding.top = (int)(defaultTop * size);
            if (resizableBottom)
                LayoutGroup.padding.bottom = (int)(defaultBottom * size);
            if (resizableSpacing)
                LayoutGroup.spacing = defaultSpacing * size;
        }

        protected virtual void OnDestroy()
        {
            if (Resizer) Resizer.Resized -= Resized;
        }
    }
}
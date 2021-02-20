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
        [Inject]
        public virtual void Inject(CanvasResizer resizer)
        {
            LayoutGroup = GetComponent<HorizontalOrVerticalLayoutGroup>();
            defaultLeft = LayoutGroup.padding.left;
            defaultRight = LayoutGroup.padding.right;
            defaultTop = LayoutGroup.padding.top;
            defaultBottom = LayoutGroup.padding.bottom;
            defaultSpacing = LayoutGroup.spacing;

            resizer.Resized += Resized;
            Resized(resizer.ResizeValue);
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
    }
}
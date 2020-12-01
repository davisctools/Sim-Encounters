using UnityEngine;

namespace ClinicalTools.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class ControlDropdownBorders : MonoBehaviour
    {
        [SerializeField] private RectTransform controlledRect = null;
        [SerializeField] private GameObject[] hide = new GameObject[0];

        protected RectTransform RectTrans => (RectTransform)transform;

        protected RectTransform ParentTrans { get; set; }
        protected CanvasGroup ControlledGroup { get; set; }
        protected CanvasGroup Group { get; set; }

        private bool started;
        protected virtual void Start()
        {
            started = true;
            ControlledGroup = controlledRect.GetComponent<CanvasGroup>();
            ParentTrans = (RectTransform)controlledRect.parent;
            Group = GetComponent<CanvasGroup>();
        }

        protected virtual void OnRectTransformDimensionsChange()
        {
            if (!started)
                Start();

            controlledRect.pivot = RectTrans.pivot;
            controlledRect.anchorMax = RectTrans.anchorMax;
            controlledRect.anchorMin = RectTrans.anchorMin;

            var height = Mathf.Min(RectTrans.rect.height, ParentTrans.rect.height);
            controlledRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);

            var alpha = (controlledRect.rect.height < 10) ? 0 : 1;
            Group.alpha = alpha;
            ControlledGroup.alpha = alpha;
        }

        protected virtual void Update()
        {
            var shouldHide = Group.alpha >= .5f;
            foreach (var hideObj in hide)
                hideObj.SetActive(shouldHide);
        }
    }
}
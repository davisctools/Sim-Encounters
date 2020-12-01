using UnityEngine;
using UnityEngine.EventSystems;

namespace ClinicalTools.UI
{
    [ExecuteAlways]
    public class AnchorsToMatchAspectRatio : UIBehaviour
    {
        [SerializeField] private Vector2 aspectRatio = Vector2.one;

        protected RectTransform RectTransform => (RectTransform)transform;

        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();
            Abc();
        }

        protected override void Start()
        {
            base.Start();
            Abc();
        }

        protected virtual void Abc()
        {
            var parentRect = ((RectTransform)transform.parent).rect;

            var proportion = aspectRatio.x / aspectRatio.y;


            var width = parentRect.height * proportion;
            var halfProportionWidth = (1f - width / parentRect.width) / 2f;
            if (halfProportionWidth > 0) {
                RectTransform.anchorMin = new Vector2(halfProportionWidth, 0);
                RectTransform.anchorMax = new Vector2(1f - halfProportionWidth, 1);
            } else {
                RectTransform.anchorMin = Vector2.zero;
                RectTransform.anchorMax = Vector2.one;
            }
        }
    }
}
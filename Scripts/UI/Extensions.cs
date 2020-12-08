using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ClinicalTools.UI.Extensions
{
    public static class Extensions
    {
        public static void AddOnSelectListener(this Toggle toggle, UnityAction action)
        {
            toggle.onValueChanged.AddListener(
                (selected) => {
                    if (selected) action();
                }
            );
        }

        public static void EnsureChildIsShowing(this ScrollRect scrollRect, RectTransform childTransform)
        {
            if (scrollRect.horizontal)
                scrollRect.EnsureHorizontalChildIsShowing(childTransform, scrollRect.horizontalNormalizedPosition);
            if (scrollRect.vertical)
                scrollRect.EnsureVerticalChildIsShowing(childTransform, scrollRect.verticalNormalizedPosition);
        }

        public static void EnsureVerticalChildIsShowing(this ScrollRect scrollRect, RectTransform childTransform, float currentPosition)
        {
            scrollRect.verticalNormalizedPosition =
                GetNormalizedPositionFromEnd(scrollRect.viewport.rect.height, scrollRect.content.rect.height,
                scrollRect.content.pivot.y, childTransform.localPosition.y, childTransform.rect.height,
                currentPosition);
        }
        private static float GetNormalizedPositionFromEnd(float viewportLength, float contentLength,
            float contentPivot, float childLocalPosition, float childLength, float currentNormalizedPosition)
        {
            if (viewportLength < 0)
                return currentNormalizedPosition;

            var scrollableDistance = contentLength - viewportLength;
            if (scrollableDistance < 0)
                return currentNormalizedPosition;

            var contentEnd = contentLength * contentPivot;
            var childPositionFromEnd = contentEnd + childLocalPosition;
            var childStart = (childPositionFromEnd - childLength) / scrollableDistance;
            var childEnd = (childPositionFromEnd - viewportLength) / scrollableDistance;

            return Mathf.Clamp(currentNormalizedPosition, childEnd, childStart);
        }

        public static void EnsureHorizontalChildIsShowing(this ScrollRect scrollRect, RectTransform childTransform, float currentPosition)
        {
            var pos =
                GetNormalizedPositionFromStart(scrollRect.viewport.rect.width, scrollRect.content.rect.width,
                childTransform.localPosition.x, childTransform.rect.width, currentPosition);
            //if (Mathf.Abs(currentPosition - pos) > .001f)
            scrollRect.horizontalNormalizedPosition = pos;
        }
        private static float GetNormalizedPositionFromStart(float viewportLength, float contentLength,
            float childLocalPosition, float childLength, float currentNormalizedPosition)
        {
            if (viewportLength < 0)
                return currentNormalizedPosition;

            var scrollableDistance = contentLength - viewportLength;
            if (scrollableDistance < 0)
                return currentNormalizedPosition;

            var childStart = childLocalPosition / scrollableDistance;
            var childEnd = (childLocalPosition - viewportLength + childLength) / scrollableDistance;

            return Mathf.Clamp(currentNormalizedPosition, childEnd, childStart);
        }
    }
}

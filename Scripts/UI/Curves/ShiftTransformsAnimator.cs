using System;
using System.Collections;
using UnityEngine;

namespace ClinicalTools.UI
{
    public class ShiftTransformsAnimator : IShiftTransformsAnimator
    {
        protected ICurve Curve { get; set; }
        public ShiftTransformsAnimator(ICurve curve) => Curve = curve;

        public virtual void SetPosition(RectTransform current)
        {
            current.anchorMin = new Vector2(0, current.anchorMin.y);
            current.anchorMax = new Vector2(1, current.anchorMax.y);
        }

        public virtual IEnumerator ShiftForward(RectTransform leaving, RectTransform current)
            => Shift(SetMoveAmountForward, leaving, current);
        public virtual IEnumerator ShiftBackward(RectTransform leaving, RectTransform current)
            => Shift(SetMoveAmountBackward, leaving, current);

        private const float MoveTime = .5f;
        protected virtual IEnumerator Shift(Action<RectTransform, RectTransform, float> moveAction,
            RectTransform leaving, RectTransform current)
        {
            var moveAmount = Mathf.Clamp01(Curve.GetCurveX(Mathf.Abs(leaving.anchorMin.x)));

            while (moveAmount < 1f) {
                moveAmount += Time.deltaTime / MoveTime;
                var position = Curve.GetCurveY(moveAmount);
                moveAction(leaving, current, position);
                yield return null;
            }

            SetPosition(current);
            leaving.gameObject.SetActive(false);
        }

        public virtual void SetMoveAmountForward(RectTransform leaving, RectTransform current, float moveAmount)
        {
            leaving.anchorMin = new Vector2(0 - moveAmount, leaving.anchorMin.y);
            leaving.anchorMax = new Vector2(1 - moveAmount, leaving.anchorMax.y);
            current.anchorMin = new Vector2(1 - moveAmount, leaving.anchorMin.y);
            current.anchorMax = new Vector2(2 - moveAmount, leaving.anchorMax.y);
        }

        public virtual void SetMoveAmountBackward(RectTransform leaving, RectTransform current, float moveAmount)
        {
            leaving.anchorMin = new Vector2(0 + moveAmount, leaving.anchorMin.y);
            leaving.anchorMax = new Vector2(1 + moveAmount, leaving.anchorMax.y);
            current.anchorMin = new Vector2(-1 + moveAmount, leaving.anchorMin.y);
            current.anchorMax = new Vector2(0 + moveAmount, leaving.anchorMax.y);
        }
    }
}
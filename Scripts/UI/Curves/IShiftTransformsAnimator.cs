using System.Collections;
using UnityEngine;

namespace ClinicalTools.UI
{
    public interface IShiftTransformsAnimator
    {
        IEnumerator ShiftForward(RectTransform leaving, RectTransform current);
        IEnumerator ShiftBackward(RectTransform leaving, RectTransform current);
        void SetPosition(RectTransform current);
        void SetMoveAmountForward(RectTransform leaving, RectTransform current, float moveAmount);
        void SetMoveAmountBackward(RectTransform leaving, RectTransform current, float moveAmount);
    }
}
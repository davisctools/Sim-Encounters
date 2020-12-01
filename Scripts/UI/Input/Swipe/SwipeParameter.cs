using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ClinicalTools.UI
{
    public class SwipeParameter
    {
        public Rect? StartPositionRange { get; set; }
        public List<AngleRange> AngleRanges { get; } = new List<AngleRange>();

        public event Action<Swipe> OnSwipeStart;
        public event Action<Swipe> OnSwipeUpdate;
        public event Action<Swipe> OnSwipeEnd;

        public bool MeetsParamaters(Swipe swipe)
        {
            if (StartPositionRange?.Contains(swipe.StartPosition) == false)
                return false;

            return (AngleRanges.Count == 0 || AngleRanges.Any(a => a.ContainsAngle(swipe.InitialAngle)));
        }
        public void SwipeStart(Swipe swipe) => OnSwipeStart?.Invoke(swipe);
        public void SwipeUpdate(Swipe swipe) => OnSwipeUpdate?.Invoke(swipe);
        public void SwipeEnd(Swipe swipe) => OnSwipeEnd?.Invoke(swipe);
    }
}

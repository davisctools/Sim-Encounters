using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ClinicalTools.UI
{
    public class SwipeManager : MonoBehaviour
    {
        private int itemsDisablingSwipe = 0;
        public void ReenableSwipe()
        {
            if (itemsDisablingSwipe > 0)
                itemsDisablingSwipe--;
        }
        public void DisableSwipe() => itemsDisablingSwipe++;
        public bool SwipeAllowed => itemsDisablingSwipe <= 0;

        private HashSet<SwipeParameter> SwipeParameters { get; } = new HashSet<SwipeParameter>();

        public void AddSwipeAction(SwipeParameter swipeParameter)
        {
            if (!SwipeParameters.Contains(swipeParameter))
                SwipeParameters.Add(swipeParameter);
        }
        public void RemoveSwipeAction(SwipeParameter swipeParameter)
        {
            if (SwipeParameters.Contains(swipeParameter))
                SwipeParameters.Remove(swipeParameter);
        }
        private void Update()
        {
            if (SwipeAllowed && (Input.touches.Length == 1 || (Input.touches.Length == 0 && Input.GetMouseButton(0))))
                TouchPosition(GetTouchPosition());
            else if (startPosition != null)
                FinishSwipe();
        }

        protected virtual Vector2 GetTouchPosition()
            => (Input.touches.Length == 1) ? Input.touches[0].position : (Vector2)Input.mousePosition;


        public bool IsSwiping() => currentSwipe != null;
        private Vector2? startPosition;
        private Swipe currentSwipe;
        protected List<SwipeParameter> CurrentParameters { get; } = new List<SwipeParameter>();
        public void TouchPosition(Vector2 position)
        {
            if (startPosition == null) {
                startPosition = position;
                return;
            }

            if (currentSwipe != null) {
                currentSwipe.LastPosition = position;
                foreach (var parameter in CurrentParameters)
                    parameter.SwipeUpdate(currentSwipe);
                return;
            }

            var currentDistance = ((Vector2)startPosition) - position;
            currentDistance.x = Mathf.Abs(currentDistance.x);
            currentDistance.y = Mathf.Abs(currentDistance.y);
            if (currentDistance.x < (Screen.width * .01f) && currentDistance.y < (Screen.height * .01f))
                return;

            currentSwipe = new Swipe((Vector2)startPosition, position);
            foreach (var parameter in SwipeParameters.Where(p => p.MeetsParamaters(currentSwipe)))
                CurrentParameters.Add(parameter);
            foreach (var parameter in CurrentParameters)
                parameter.SwipeStart(currentSwipe);
        }

        public void FinishSwipe()
        {
            startPosition = null;
            if (currentSwipe == null)
                return;

            foreach (var parameter in CurrentParameters)
                parameter.SwipeEnd(currentSwipe);
            CurrentParameters.Clear();

            currentSwipe = null;
        }
    }
}

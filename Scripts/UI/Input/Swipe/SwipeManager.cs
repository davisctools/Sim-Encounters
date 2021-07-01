using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ClinicalTools.UI
{
    public class SwipeLayer
    {
        public HashSet<SwipeParameter> SwipeParameters { get; protected set; } = new HashSet<SwipeParameter>();
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

        public Rect ScreenProportionRect { get; set; } = new Rect(0, 0, 1, 1);

    }

    public class SwipeManager : MonoBehaviour
    {
        private int itemsDisablingSwipe = 0;
        public void ReenableSwipe()
        {
            if (itemsDisablingSwipe > 0)
                itemsDisablingSwipe--;
        }
        public void DisableSwipe()
        {
            itemsDisablingSwipe++;
        }

        public bool SwipeAllowed => itemsDisablingSwipe <= 0;

        protected SwipeLayer DefaultLayer { get; } = new SwipeLayer();
        public List<SwipeLayer> SwipeLayers { get; } = new List<SwipeLayer>();
        public virtual void AddSwipeLayer(SwipeLayer swipeLayer) => SwipeLayers.Add(swipeLayer);
        public virtual void RemoveSwipeLayer(SwipeLayer swipeLayer)
        {
            if (SwipeLayers.Contains(swipeLayer))
                SwipeLayers.Remove(swipeLayer);
        }

        public virtual void RemoveSwipeLayer()
        {
            if (SwipeLayers.Count == 0) {

            }
        }

        public void AddSwipeAction(SwipeParameter swipeParameter) => DefaultLayer.AddSwipeAction(swipeParameter);
        public void RemoveSwipeAction(SwipeParameter swipeParameter) => DefaultLayer.RemoveSwipeAction(swipeParameter);
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
        private SwipeLayer swipeLayer;
        private Vector2? startPosition;
        private Swipe currentSwipe;
        protected List<SwipeParameter> CurrentParameters { get; } = new List<SwipeParameter>();
        public void TouchPosition(Vector2 position)
        {
            if (startPosition == null)
                StartSwipe(position);
            else if (currentSwipe != null)
                UpdateCurrentSwipe(position);
            else if (IsPositionASwipe(position))
                InitializeSwipe(position);
        }

        protected virtual void StartSwipe(Vector2 position)
        {
            startPosition = position;
            swipeLayer = GetSwipeLayer(position);
        }

        protected virtual void UpdateCurrentSwipe(Vector2 position)
        {
            currentSwipe.LastPosition = position;
            foreach (var parameter in CurrentParameters)
                parameter.SwipeUpdate(currentSwipe);
        }

        protected virtual bool IsPositionASwipe(Vector2 position)
        {
            var currentDistance = ((Vector2)startPosition) - position;
            currentDistance.x = Mathf.Abs(currentDistance.x);
            currentDistance.y = Mathf.Abs(currentDistance.y);
            return currentDistance.x >= (Screen.width * .01f) || currentDistance.y >= (Screen.height * .01f);
        }

        protected virtual void InitializeSwipe(Vector2 position)
        {
            currentSwipe = new Swipe((Vector2)startPosition, position);
            foreach (var parameter in swipeLayer.SwipeParameters.Where(p => p.MeetsParamaters(currentSwipe)))
                CurrentParameters.Add(parameter);
            foreach (var parameter in CurrentParameters)
                parameter.SwipeStart(currentSwipe);
        }


        protected virtual SwipeLayer GetSwipeLayer(Vector2 position)
        {
            var x = position.x / Screen.width;
            var y = position.y / Screen.height;

            for (int i = SwipeLayers.Count - 1; i >= 0; i--) {
                var swipeLayer = SwipeLayers[i];
                var layerRect = swipeLayer.ScreenProportionRect;
                if (layerRect.x <= x && layerRect.x + layerRect.width >= x && 
                    layerRect.y <= y && layerRect.y + layerRect.height >= y) {
                    return swipeLayer;
                }
            }

            return DefaultLayer;
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

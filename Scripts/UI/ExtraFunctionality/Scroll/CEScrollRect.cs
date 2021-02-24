using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ClinicalTools.UI
{
    public class CEScrollRect : ScrollRect
    {
        private const float SNAP_BUFFER = 10;

        protected override void Awake()
        {
            // The movement is restricted in LateUpdate by this script
            // The restriction acts like clamped, but without restarting from the top when it moves too far down.
            movementType = MovementType.Unrestricted;

            base.Awake();
        }

        /// <summary>
        /// Moves the scrollrect so that the top target is in sight if it was previously below or the bottom target is in sight if it was previously above.
        /// </summary>
        /// <param name="topTarget">Topmost rectangle that should be in sight.</param>
        /// <param name="botTarget">Bottommost rectangle that should be in sight.</param>
        public void SnapTo(RectTransform topTarget, RectTransform botTarget)
        {
            Canvas.ForceUpdateCanvases();

            Vector3[] v = new Vector3[4];

            topTarget.GetWorldCorners(v);
            Vector3 topCorner = v[1];
            if (viewport.transform.InverseTransformPoint(topCorner).y > 0) {
                SnapToTopTarget(topCorner);
                return;
            }

            botTarget.GetWorldCorners(v);
            Vector3 bottomCorner = v[0];
            if (-viewport.transform.InverseTransformPoint(bottomCorner).y > viewport.rect.height)
                SnapToBottomTarget(bottomCorner);
        }

        protected virtual void SnapToTopTarget(Vector3 topCorner)
        {
            var pos = GetCornerPosition(topCorner);
            pos.y -= SNAP_BUFFER;
            content.anchoredPosition = pos;
        }
        protected virtual void SnapToBottomTarget(Vector3 bottomCorner)
        {
            var pos = GetCornerPosition(bottomCorner);
            pos.y -= viewport.rect.height;
            pos.y += SNAP_BUFFER;

            content.anchoredPosition = pos;
        }
        protected virtual Vector2 GetCornerPosition(Vector3 corner)
        {
            Vector2 pos =
                (Vector2)transform.InverseTransformPoint(content.position)
                - (Vector2)transform.InverseTransformPoint(corner);
            pos.x = content.anchoredPosition.x;
            return pos;
        }

        public override void OnScroll(PointerEventData data)
        {
            if (content.rect.height > viewport.rect.height || content.rect.width > viewport.rect.width)
                base.OnScroll(data);
        }

        private bool setNextYPos, setNextXPos;
        private float nextY, nextX;
        protected override void LateUpdate()
        {
            if (setNextXPos) {
                SetXPosition(nextX);
                setNextXPos = false;
            }
            if (setNextYPos) {
                SetYPosition(nextY);
                setNextYPos = false;
            }

            var maxX = GetMaxValue(content.rect.width - viewport.rect.width);
            var maxY = GetMaxValue(content.rect.height - viewport.rect.height);
            if (horizontal)
                InitialUpdateX(maxX);
            if (vertical)
                InitialUpdateY(maxY);

            base.LateUpdate();

            if (horizontal)
                LateUpdateX(maxX);
            if (vertical)
                LateUpdateY(maxY);
        }

        protected virtual float GetMaxValue(float contentViewportSizeDifference)
        {
            // It needs a buffer to prevent looping when getting to the end. The buffer needed seems to scale with the heights
            // A scale of .9999f seems to work well without revealing that you can't scroll to the very end. .99999f produces too small a buffer
            // Unfortunately this doesn't work for very small ones, so a hard buffer of .02 is used
            var buffer = contentViewportSizeDifference * .0001f;
            if (buffer < .02f)
                buffer = .02f;
            return contentViewportSizeDifference - buffer;
        }

        protected virtual void InitialUpdateX(float maxX)
            => InitialUpdatePosition(SetXPosition, content.anchoredPosition.x, maxX, content.anchorMin.x);
        protected virtual void InitialUpdateY(float maxY)
            => InitialUpdatePosition(SetYPosition, content.anchoredPosition.y, maxY, content.anchorMin.y);

        protected virtual void InitialUpdatePosition(SetPosition setPosition, float position, float maxPosition, float anchorMin)
        {
            if (anchorMin > .5f) {
                if (position < 0)
                    setPosition(0, true);
                else if (maxPosition > 1f && position > maxPosition)
                    setPosition(maxPosition);
            } else {
                if (maxPosition > 1f && position < -maxPosition)
                    setPosition(-maxPosition, true);
                else if (position > 0)
                    setPosition(0);
            }
        }

        protected virtual void LateUpdateX(float maxX)
            => LateUpdatePosition(SetXPosition, content.anchoredPosition.x, maxX, content.anchorMin.x);
        protected virtual void LateUpdateY(float maxY)
            => LateUpdatePosition(SetYPosition, content.anchoredPosition.y, maxY, content.anchorMin.y);
        protected virtual void LateUpdatePosition(SetPosition setPosition, float position, float maxPosition, float anchorMin)
        {
            float nextPosition;
            if (anchorMin > .5f) {
                if (position < 0)
                    nextPosition = 0;
                else if (maxPosition > 1f && position > maxPosition)
                    nextPosition = maxPosition;
                else
                    return;
            } else {
                if (maxPosition > 1f && position < -maxPosition)
                    nextPosition = -maxPosition;
                else if (position > 0)
                    nextPosition = 0;
                else
                    return;
            }

            setPosition(nextPosition, true, true);
        }

        public delegate void SetPosition(float position, bool resetVelocity = false, bool setNextPosition = false);
        protected virtual void SetXPosition(float x, bool resetVelocity = false, bool setNextPosition = false)
        {
            if (resetVelocity)
                velocity = new Vector2();
            if (setNextPosition) { 
                setNextXPos = true;
                nextX = x;
            }

            content.anchoredPosition = new Vector2(x, content.anchoredPosition.y);
        }

        protected virtual void SetYPosition(float y, bool resetVelocity = false, bool setNextPosition = false)
        {
            if (resetVelocity)
                velocity = new Vector2();
            if (setNextPosition) { 
                setNextYPos = true;
                nextY = y;
            }

            content.anchoredPosition = new Vector2(content.anchoredPosition.x, y);
        }
    }
}
using UnityEngine;
using UnityEngine.UI;
using static ClinicalTools.SimEncounters.ISwipableSection;

namespace ClinicalTools.SimEncounters
{
    public class HorizontalUserSectionToggleGroup : UserSectionToggleGroup, ISwipableSection
    {
        [SerializeField] private ScrollRect scrollRect;

        protected override void SectionSelected(object sender, UserSectionSelectedEventArgs e)
        {
            base.SectionSelected(sender, e);

            UpdatePosition(e.SelectedSection);
            shouldUpdate = true;
        }

        private bool shouldUpdate;
        protected virtual void LateUpdate()
        {
            if (!shouldUpdate)
                return;

            shouldUpdate = false;
            UpdatePosition(SectionSelector.CurrentValue.SelectedSection);
        }

        protected virtual void UpdatePosition(UserSection section)
        {
            Canvas.ForceUpdateCanvases();

            var contentCorners = new Vector3[4];
            scrollRect.content.GetWorldCorners(contentCorners);

            var contentWidth = GetWidth(contentCorners);
            var viewportWidth = GetWidth(scrollRect.viewport);
            var divisor = contentWidth - viewportWidth;
            if (divisor <= 0)
                return;

            var buttonX = GetButtonX((RectTransform)SectionButtons[section].transform, contentCorners);
            var dividend = buttonX - viewportWidth / 2;
            scrollRect.horizontalNormalizedPosition = Mathf.Clamp01(dividend / divisor);
        }

        protected virtual float GetButtonX(RectTransform buttonTransform, Vector3[] contentCorners)
        {
            var buttonCorners = new Vector3[4];
            buttonTransform.GetWorldCorners(buttonCorners);
            var buttonWidth = GetWidth(buttonCorners);

            return buttonCorners[0].x - contentCorners[0].x + buttonWidth / 2;
        }
        protected virtual float GetWidth(RectTransform rectTransform)
        {
            var corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);
            return GetWidth(corners);
        }
        protected virtual float GetWidth(Vector3[] corners) => corners[2].x - corners[0].x;

        public void SwipeStart()
        {
            throw new System.NotImplementedException();
        }

        public void SwipeUpdate(Direction dir, float dist)
        {
            throw new System.NotImplementedException();
        }

        public void SwipeEnd(Direction dir, float dist, bool changingSections)
        {
            throw new System.NotImplementedException();
        }

        private float lastPosition;
        private float currentPosition;
        private float nextPosition;
        protected virtual void Show()
        {

        }
    }
}
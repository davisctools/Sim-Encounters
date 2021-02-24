using UnityEngine;
using UnityEngine.UI;
using static ClinicalTools.SimEncounters.ReaderGeneralSectionHandler;

namespace ClinicalTools.SimEncounters
{
    public class HorizontalUserSectionToggleGroup : UserSectionToggleGroup, ISwipableSection
    {
        [SerializeField] private ScrollRect scrollRect;

        protected override void SectionSelected(object sender, UserSectionSelectedEventArgs e)
        {
            base.SectionSelected(sender, e);
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
            StartMove();
            scrollRect.horizontalNormalizedPosition = GetSectionHorizontalNormalizedPosition(section);
        }

        protected virtual float GetSectionHorizontalNormalizedPosition(UserSection section)
        {
            if (section == null)
                return 0;

            var contentCorners = new Vector3[4];
            scrollRect.content.GetWorldCorners(contentCorners);

            var contentWidth = GetWidth(contentCorners);
            var viewportWidth = GetWidth(scrollRect.viewport);
            var divisor = contentWidth - viewportWidth;
            if (divisor <= 0)
                return 0;

            var buttonX = GetButtonX((RectTransform)SectionButtons[section].transform, contentCorners);
            var dividend = buttonX - viewportWidth / 2;

            return Mathf.Clamp01(dividend / divisor);
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


        protected float CurrentSectionPosition { get; set; }
        protected float NextSectionPosition { get; set; }
        protected float PreviousSectionPosition { get; set; }
        protected UserSection CurrentMoveSection { get; set; }
        protected UserSection NextMoveSection { get; set; }
        protected UserSection PreviousMoveSection { get; set; }
        public void StartMove()
        {
            var encounter = EncounterSelector.CurrentValue.Encounter;
            var content = encounter.Data.Content.NonImageContent;
            var sections = encounter.Sections;
            var currentIndex = content.CurrentSectionIndex;
            CurrentMoveSection = sections[currentIndex].Value;
            NextMoveSection = (currentIndex + 1 < sections.Count) ? sections[currentIndex + 1].Value : null;
            PreviousMoveSection = (currentIndex > 0) ? sections[currentIndex - 1].Value : null;
        }

        protected virtual void UpdatePositionPoints()
        {
            CurrentSectionPosition = GetSectionHorizontalNormalizedPosition(CurrentMoveSection);
            NextSectionPosition = GetSectionHorizontalNormalizedPosition(NextMoveSection);
            PreviousSectionPosition = GetSectionHorizontalNormalizedPosition(PreviousMoveSection);
        }

        public void Move(Direction dir, float dist)
        {
            UpdatePositionPoints();
            if (dir == Direction.NA) {
                scrollRect.horizontalNormalizedPosition = CurrentSectionPosition;
                return;
            }

            var desiredPosition = dir == Direction.Left ? NextSectionPosition : PreviousSectionPosition;
            
            if (desiredPosition == CurrentSectionPosition) {
                scrollRect.horizontalNormalizedPosition = CurrentSectionPosition;
                return;
            }

            // We're given the distance from the start, but we want the distance from the goal
            dist = 1 - dist;
            scrollRect.horizontalNormalizedPosition = desiredPosition - (dist * (desiredPosition - CurrentSectionPosition));
        }

        public void EndMove()
        {
            UpdatePosition(SectionSelector.CurrentValue.SelectedSection);
        }
    }
}
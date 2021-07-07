using ClinicalTools.UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using static ClinicalTools.SimEncounters.ReaderGeneralSectionHandler;

namespace ClinicalTools.SimEncounters
{
    public class HorizontalUserSectionToggleGroup : UserSectionToggleGroup, ISwipableSection
    {
        [SerializeField] private ScrollRect scrollRect;

        protected ICurve Curve { get; set; }
        [Inject] public virtual void Inject(ICurve curve) => Curve = curve;

        protected UserSection CurrentSection { get; set; }
        protected UserSection NextSection { get; set; }
        protected UserSection PreviousSection { get; set; }
        protected override void OnSectionSelected(object sender, UserSectionSelectedEventArgs e)
        {
            if (CurrentSection == e.SelectedSection)
                return;

            var encounter = EncounterSelector.CurrentValue.Encounter;
            var content = encounter.Data.Content.NonImageContent;
            var sections = encounter.Sections;
            var currentIndex = content.CurrentSectionIndex;
            CurrentSection = e.SelectedSection;
            NextSection = (currentIndex + 1 < sections.Count) ? sections[currentIndex + 1].Value : null;
            PreviousSection = (currentIndex > 0) ? sections[currentIndex - 1].Value : null;

            if (e.ChangeType != ChangeType.Next && e.ChangeType != ChangeType.Previous)
                UpdatePosition(CurrentSection);

            base.OnSectionSelected(sender, e);
        }

        private bool shouldUpdate;
        protected virtual void LateUpdate()
        {
            CheckContentSizeChanged();

            if (!shouldUpdate)
                return;

            shouldUpdate = false;
            SetScrollPosition(GetSectionHorizontalNormalizedPosition(SectionSelector.CurrentValue.SelectedSection));

            lastContentPosition = scrollRect.horizontalNormalizedPosition;
        }

        private bool contentMoved = true;
        private float lastContentPosition;
        private float lastContentWidth;
        protected virtual void CheckContentSizeChanged()
        {
            if (Mathf.Abs(scrollRect.content.rect.width - lastContentWidth) < .001f)
                contentMoved = Mathf.Abs(scrollRect.horizontalNormalizedPosition - lastContentPosition) > .001f;
            else
                OnContentSizeChanged();
        }

        protected virtual void OnContentSizeChanged()
        {
            lastContentWidth = scrollRect.content.rect.width;

            if (contentMoved)
                return;

            shouldUpdate = true;
        }


        protected virtual void UpdatePosition(UserSection section)
        {
            Canvas.ForceUpdateCanvases();
            shouldUpdate = true;
            SetScrollPosition(GetSectionHorizontalNormalizedPosition(section));
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

        protected float MaxTime { get; } = .5f;
        protected float TimeUntilBack { get; set; } = 0;
        protected float InitialDistance { get; set; } = 0;
        protected float CurrentSectionPosition { get; set; }
        protected float NextSectionPosition { get; set; }
        protected float PreviousSectionPosition { get; set; }
        public void StartMove()
        {
            UpdatePositionPoints();
            if (Mathf.Abs(CurrentSectionPosition - scrollRect.horizontalNormalizedPosition) > .001f) {
                TimeUntilBack = MaxTime;
                InitialDistance = CurrentSectionPosition - scrollRect.horizontalNormalizedPosition;
            }
            LastDirection = Direction.NA;
        }

        protected virtual void UpdatePositionPoints()
        {
            CurrentSectionPosition = GetSectionHorizontalNormalizedPosition(CurrentSection);
            NextSectionPosition = GetSectionHorizontalNormalizedPosition(NextSection);
            PreviousSectionPosition = GetSectionHorizontalNormalizedPosition(PreviousSection);
        }

        protected Direction LastDirection { get; set; }
        public void Move(Direction dir, float dist)
        {
            if (CurrentSection != SectionSelector.CurrentValue.SelectedSection)
                OnSectionSelected(SectionSelector, SectionSelector.CurrentValue);

            UpdatePositionPoints();
            if (dir == Direction.NA) {
                if (LastDirection != Direction.NA)
                    SetPositionInMove(CurrentSectionPosition);
                LastDirection = dir;
                return;
            }

            var desiredPosition = dir == Direction.Next ? NextSectionPosition : PreviousSectionPosition;

            if (desiredPosition == CurrentSectionPosition) {
                SetPositionInMove(CurrentSectionPosition);
                return;
            }

            // We're given the distance from the start, but we want the distance from the goal
            dist = 1 - dist;
            SetPositionInMove(desiredPosition - (dist * (desiredPosition - CurrentSectionPosition)));
        }

        protected virtual void SetPositionInMove(float position)
        {
            SetScrollPosition(position);
            lastContentPosition = scrollRect.horizontalNormalizedPosition;
            if (TimeUntilBack <= 0)
                return;

            SetScrollPosition(scrollRect.horizontalNormalizedPosition - Curve.GetCurveY(TimeUntilBack / MaxTime) * InitialDistance);
            TimeUntilBack -= Time.deltaTime;
        }

        protected virtual void SetScrollPosition(float position)
        {
            scrollRect.horizontalNormalizedPosition = position;
            lastContentPosition = scrollRect.horizontalNormalizedPosition;
        }

        public void EndMove()
        {
            if (CurrentSection != SectionSelector.CurrentValue.SelectedSection)
                OnSectionSelected(SectionSelector, SectionSelector.CurrentValue);

            if (LastDirection != Direction.NA)
                UpdatePosition(SectionSelector.CurrentValue.SelectedSection);
        }
    }
}
using ClinicalTools.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ReaderGeneralSectionHandler : MonoBehaviour
    {
        public enum Direction { NA, Left, Right }

        [SerializeField] MonoBehaviour[] swipableSectionBehaviours;
        [SerializeField] RectTransform swipeBounds;
        List<ISwipableSection> swipableSections = new List<ISwipableSection>();

        protected AnimationMonitor AnimationMonitor { get; set; }
        protected SwipeManager SwipeManager { get; set; }
        protected ICurve Curve { get; set; }
        protected ISelectedListener<UserEncounterSelectedEventArgs> EncounterSelector { get; set; }
        protected ISelector<UserSectionSelectedEventArgs> SectionSelector { get; set; }
        [Inject]
        public virtual void Inject(
            AnimationMonitor animationMonitor,
            SwipeManager swipeManager,
            ICurve curve,
            ISelectedListener<UserEncounterSelectedEventArgs> encounterSelector,
            ISelector<UserSectionSelectedEventArgs> sectionSelector)
        {
            foreach (var behaviour in swipableSectionBehaviours) {
                if (behaviour is ISwipableSection swipableSection)
                    swipableSections.Add(swipableSection);
            }

            AnimationMonitor = animationMonitor;
            SwipeManager = swipeManager;
            Curve = curve;

            EncounterSelector = encounterSelector;
            SectionSelector = sectionSelector;
            SectionSelector.Selected += OnSectionSelected;
            if (SectionSelector.CurrentValue != null)
                OnSectionSelected(SectionSelector, SectionSelector.CurrentValue);
        }

        protected virtual void OnEnable()
        {
            if (SwipeParamater == null)
                InitializeSwipeParamaters();
            SwipeManager.AddSwipeAction(SwipeParamater);
        }
        protected virtual void OnDisable() => SwipeManager.RemoveSwipeAction(SwipeParamater);

        protected UserSection NextSection { get; set; }
        protected UserSection PreviousSection { get; set; }
        protected UserSection CurrentSection { get; set; }
        protected virtual void OnSectionSelected(object sender, UserSectionSelectedEventArgs e)
        {
            var encounter = EncounterSelector.CurrentValue.Encounter;
            var nonImageContent = encounter.Data.Content.NonImageContent;
            var sectionIndex = nonImageContent.CurrentSectionIndex;
            PreviousSection = (sectionIndex > 0) ? encounter.Sections[sectionIndex - 1].Value : null;
            NextSection = (sectionIndex + 1 < encounter.Sections.Count) ? encounter.Sections[sectionIndex + 1].Value : null;
            CurrentSection = e.SelectedSection;

            if ((object)sender == this || (e.ChangeType != ChangeType.Next && e.ChangeType != ChangeType.Previous))
                return;

            if (CurrentCoroutine != null)
                StopCoroutine(CurrentCoroutine);
            var direction = (e.ChangeType == ChangeType.Next) ? Direction.Left : Direction.Right;
            CurrentCoroutine = StartCoroutine(SectionChanged(direction));
        }

        protected SwipeParameter SwipeParamater { get; set; }
        protected virtual void InitializeSwipeParamaters()
        {
            SwipeParamater = new SwipeParameter();
            var corners = new Vector3[4];
            swipeBounds.GetWorldCorners(corners);
            SwipeParamater.StartPositionRange = new Rect(corners[0], corners[2] - corners[0]);
            SwipeParamater.AngleRanges.Add(new AngleRange(-30, 30));
            SwipeParamater.AngleRanges.Add(new AngleRange(150, 210));
            SwipeParamater.OnSwipeStart += SwipeStart;
            SwipeParamater.OnSwipeUpdate += SwipeUpdate;
            SwipeParamater.OnSwipeEnd += SwipeEnd;
        }

        protected virtual void SwipeStart(Swipe obj)
        {
            if (CurrentCoroutine != null) { 
                StopCoroutine(CurrentCoroutine);
                CurrentCoroutine = null;
            }

            StartMove();
            SwipeUpdate(obj);
        }

        protected virtual void SwipeUpdate(Swipe obj)
        {
            var dist = GetDistance(obj);
            var swipingDirection = GetDirection(dist);

            dist = Mathf.Clamp01(Mathf.Abs(dist));

            foreach (var swipableSection in swipableSections)
                swipableSection.Move(swipingDirection, dist);
        }

        protected virtual void SwipeEnd(Swipe obj)
        {
            var dist = GetDistance(obj);
            var swipingDirection = GetDirection(dist);

            if (swipingDirection == Direction.NA) {
                EndMove();
                return;
            }

            UserSectionSelectedEventArgs sectionSelectedEventArgs = GetSwipeSectionSelectedEventArgs(obj, swipingDirection, dist);
            var changingSections = sectionSelectedEventArgs != null;
            if (changingSections)
                SectionSelector.Select(this, sectionSelectedEventArgs);

            dist = Mathf.Clamp01(Mathf.Abs(dist));
            CurrentCoroutine = StartCoroutine(MoveSection(swipingDirection, dist, changingSections));
        }

        protected Coroutine CurrentCoroutine { get; set; }
        protected virtual float GetDistance(Swipe obj) => (obj.LastPosition.x - obj.StartPosition.x) / Screen.width;
        protected virtual Direction GetDirection(float dist)
        {
            var section = SectionSelector.CurrentValue.SelectedSection.Data;
            if (dist > 0 && PreviousSection != null && section.CurrentTabIndex == 0)
                return Direction.Right;
            else if (dist < 0 && NextSection != null && section.CurrentTabIndex + 1 == section.Tabs.Count)
                return Direction.Left;
            else
                return Direction.NA;
        }

        protected virtual UserSectionSelectedEventArgs GetSwipeSectionSelectedEventArgs(Swipe obj, Direction swipingDirection, float dist)
        {
            if (swipingDirection == Direction.Right && (dist > .5f || obj.Velocity.x / Screen.dpi > 1.5f))
                return new UserSectionSelectedEventArgs(PreviousSection, ChangeType.Previous);
            else if (swipingDirection == Direction.Left && (dist < -.5f || obj.Velocity.x / Screen.dpi < -1.5f))
                return new UserSectionSelectedEventArgs(NextSection, ChangeType.Next);
            else
                return null;
        }

        protected virtual float Distance { get; set; }
        protected virtual float MaxTime { get; } = .5f;

        protected virtual IEnumerator SectionChanged(Direction direction)
        {
            StartMove();
            yield return MoveSection(direction, 0, true);
        }

        protected virtual IEnumerator MoveSection(Direction direction, float initialDistance, bool changingSections)
        {
            if (!changingSections)
                initialDistance = 1 - initialDistance;

            var elapsedTime = Curve.GetCurveX(initialDistance) * MaxTime;
            do {
                elapsedTime += Time.deltaTime;
                Move(direction, elapsedTime, changingSections);
                yield return null;
            } while (elapsedTime < MaxTime);

            EndMove();
        }

        protected virtual void StartMove()
        {
            AnimationMonitor.AnimationStarting(this);
            DragOverrideScript.DragAllowed = false;
            foreach (var swipableSection in swipableSections)
                swipableSection.StartMove();
        }

        protected virtual void Move(Direction direction, float elapsedTime, bool changingSections)
        {
            var dist = Curve.GetCurveY(elapsedTime / MaxTime);
            if (!changingSections)
                dist = 1 - dist;
            foreach (var swipableSection in swipableSections)
                swipableSection.Move(direction, dist);
        }

        protected virtual void EndMove()
        {
            foreach (var swipableSection in swipableSections)
                swipableSection.EndMove();

            AnimationMonitor.AnimationStopping(this);
            DragOverrideScript.DragAllowed = true;
        }
    }
}

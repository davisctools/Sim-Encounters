using ClinicalTools.UI;
using System.Collections;
using UnityEngine;
using Zenject;
using static ClinicalTools.SimEncounters.ISwipableSection;

namespace ClinicalTools.SimEncounters
{
    public class ReaderGeneralSectionHandler : MonoBehaviour
    {
        [SerializeField] ISwipableSection[] swipableSections;

        protected SwipeManager SwipeManager { get; set; }
        protected ISelectedListener<UserEncounterSelectedEventArgs> EncounterSelector { get; set; }
        protected ISelector<UserSectionSelectedEventArgs> SectionSelector { get; set; }
        protected ISelectedListener<UserTabSelectedEventArgs> TabSelector { get; set; }
        [Inject]
        public virtual void Inject(
            SwipeManager swipeManager,
            ISelectedListener<UserEncounterSelectedEventArgs> encounterSelector,
            ISelector<UserSectionSelectedEventArgs> sectionSelector,
            ISelectedListener<UserTabSelectedEventArgs> tabSelector)
        {
            SwipeManager = swipeManager;

            EncounterSelector = encounterSelector;
            SectionSelector = sectionSelector;
            TabSelector = tabSelector;

            SectionSelector.Selected += OnSectionSelected;
            if (SectionSelector.CurrentValue != null)
                OnSectionSelected(SectionSelector, SectionSelector.CurrentValue);

            TabSelector.Selected += OnTabSelected;
            if (TabSelector.CurrentValue != null)
                OnTabSelected(TabSelector, TabSelector.CurrentValue);
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
        }

        protected bool CanSwipeLeft { get; set; }
        protected bool CanSwipeRight { get; set; }
        protected virtual void OnTabSelected(object sender, UserTabSelectedEventArgs e)
        {
            var section = SectionSelector.CurrentValue.SelectedSection.Data;
            CanSwipeLeft = NextSection != null && section.CurrentTabIndex + 1 == section.Tabs.Count;
            CanSwipeRight = PreviousSection != null && section.CurrentTabIndex == 0;
        }

        protected SwipeParameter SwipeParamater { get; set; }
        protected virtual void InitializeSwipeParamaters()
        {
            SwipeParamater = new SwipeParameter();
            SwipeParamater.AngleRanges.Add(new AngleRange(-30, 30));
            SwipeParamater.AngleRanges.Add(new AngleRange(150, 210));
            SwipeParamater.OnSwipeStart += SwipeStart;
            SwipeParamater.OnSwipeUpdate += SwipeUpdate;
            SwipeParamater.OnSwipeEnd += SwipeEnd;
        }

        protected virtual void SwipeStart(Swipe obj)
        {
            DragOverrideScript.DragAllowed = false;
            foreach (var swipableSection in swipableSections)
                swipableSection.SwipeStart();
            SwipeUpdate(obj);
        }

        protected virtual void SwipeUpdate(Swipe obj)
        {
            var dist = GetDistance(obj);
            var swipingDirection = GetDirection(dist);

            dist = Mathf.Clamp01(Mathf.Abs(dist));

            foreach (var swipableSection in swipableSections)
                swipableSection.SwipeUpdate(swipingDirection, dist);
        }

        protected virtual void SwipeEnd(Swipe obj)
        {
            DragOverrideScript.DragAllowed = true;
            var dist = GetDistance(obj);
            var swipingDirection = GetDirection(dist);

            UserSectionSelectedEventArgs sectionSelectedEventArgs = GetSwipeSectionSelectedEventArgs(obj, swipingDirection, dist);
            var changingSections = sectionSelectedEventArgs != null;
            if (changingSections)
                SectionSelector.Select(this, sectionSelectedEventArgs);

            dist = Mathf.Clamp01(Mathf.Abs(dist));
            foreach (var swipableSection in swipableSections)
                swipableSection.SwipeEnd(swipingDirection, dist, changingSections);
        }

        protected virtual float GetDistance(Swipe obj) => (obj.LastPosition.x - obj.StartPosition.x) / Screen.width;
        protected virtual Direction GetDirection(float dist)
        {
            if (dist > 0 && CanSwipeRight)
                return Direction.Right;
            else if (dist < 0 && CanSwipeLeft)
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
    }

    public class ReaderGeneralSectionHandler2 : MonoBehaviour
    {
        [SerializeField] ISwipableSection2[] swipableSections;

        protected SwipeManager SwipeManager { get; set; }
        protected ICurve Curve { get; set; }
        protected ISelectedListener<UserEncounterSelectedEventArgs> EncounterSelector { get; set; }
        protected ISelector<UserSectionSelectedEventArgs> SectionSelector { get; set; }
        protected ISelectedListener<UserTabSelectedEventArgs> TabSelector { get; set; }
        [Inject]
        public virtual void Inject(
            SwipeManager swipeManager,
            ICurve curve,
            ISelectedListener<UserEncounterSelectedEventArgs> encounterSelector,
            ISelector<UserSectionSelectedEventArgs> sectionSelector,
            ISelectedListener<UserTabSelectedEventArgs> tabSelector)
        {
            SwipeManager = swipeManager;
            Curve = curve;

            EncounterSelector = encounterSelector;
            SectionSelector = sectionSelector;
            TabSelector = tabSelector;

            SectionSelector.Selected += OnSectionSelected;
            if (SectionSelector.CurrentValue != null)
                OnSectionSelected(SectionSelector, SectionSelector.CurrentValue);

            TabSelector.Selected += OnTabSelected;
            if (TabSelector.CurrentValue != null)
                OnTabSelected(TabSelector, TabSelector.CurrentValue);
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
            CurrentCoroutine = StartCoroutine(Abc(direction, 0));
        }

        protected bool CanSwipeLeft { get; set; }
        protected bool CanSwipeRight { get; set; }
        protected virtual void OnTabSelected(object sender, UserTabSelectedEventArgs e)
        {
            var section = SectionSelector.CurrentValue.SelectedSection.Data;
            CanSwipeLeft = NextSection != null && section.CurrentTabIndex + 1 == section.Tabs.Count;
            CanSwipeRight = PreviousSection != null && section.CurrentTabIndex == 0;
        }

        protected SwipeParameter SwipeParamater { get; set; }
        protected virtual void InitializeSwipeParamaters()
        {
            SwipeParamater = new SwipeParameter();
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

            DragOverrideScript.DragAllowed = false;
            foreach (var swipableSection in swipableSections)
                swipableSection.StartMove();
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

            UserSectionSelectedEventArgs sectionSelectedEventArgs = GetSwipeSectionSelectedEventArgs(obj, swipingDirection, dist);
            var changingSections = sectionSelectedEventArgs != null;
            if (changingSections)
                SectionSelector.Select(this, sectionSelectedEventArgs);

            dist = Mathf.Clamp01(Mathf.Abs(dist));
            CurrentCoroutine = StartCoroutine(Abc(swipingDirection, dist));
        }

        protected Coroutine CurrentCoroutine { get; set; }
        protected virtual float GetDistance(Swipe obj) => (obj.LastPosition.x - obj.StartPosition.x) / Screen.width;
        protected virtual Direction GetDirection(float dist)
        {
            if (dist > 0 && CanSwipeRight)
                return Direction.Right;
            else if (dist < 0 && CanSwipeLeft)
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
        protected virtual float ElapsedTime { get; set; }
        protected virtual IEnumerator Abc(Direction direction, float initialDistance)
        {
            ElapsedTime = Curve.GetCurveX(initialDistance) * MaxTime;
            while (ElapsedTime < MaxTime) {
                ElapsedTime += Time.deltaTime;
                var dist = Curve.GetCurveY(ElapsedTime / MaxTime);
                foreach (var swipableSection in swipableSections)
                    swipableSection.Move(direction, dist);
                yield return null;
            }

            foreach (var swipableSection in swipableSections)
                swipableSection.EndMove();

            DragOverrideScript.DragAllowed = true;
        }
    }
}

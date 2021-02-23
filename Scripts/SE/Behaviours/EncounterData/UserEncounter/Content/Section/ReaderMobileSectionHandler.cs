using ClinicalTools.Collections;
using ClinicalTools.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using static ClinicalTools.SimEncounters.ISwipableSection;

namespace ClinicalTools.SimEncounters
{
    public class ReaderMobileSectionHandler : MonoBehaviour, ISwipableSection
    {
        public CanvasGroup CanvasGroup { get => canvasGroup; set => canvasGroup = value; }
        [SerializeField] private CanvasGroup canvasGroup;
        public ReaderSectionContent SectionDrawer1 { get => sectionDrawer1; set => sectionDrawer1 = value; }
        [SerializeField] private ReaderSectionContent sectionDrawer1;
        public ReaderSectionContent SectionDrawer2 { get => sectionDrawer2; set => sectionDrawer2 = value; }
        [SerializeField] private ReaderSectionContent sectionDrawer2;
        public ReaderSectionContent SectionDrawer3 { get => sectionDrawer3; set => sectionDrawer3 = value; }
        [SerializeField] private ReaderSectionContent sectionDrawer3;
        public ReaderSectionContent SectionDrawer4 { get => sectionDrawer4; set => sectionDrawer4 = value; }
        [SerializeField] private ReaderSectionContent sectionDrawer4;

        protected ReaderSectionContent[] Contents { get; } = new ReaderSectionContent[4];

        protected virtual void Awake()
        {
            Contents[0] = SectionDrawer1;
            Contents[1] = SectionDrawer2;
            Contents[2] = SectionDrawer3;
            Contents[3] = SectionDrawer4;
        }

        protected ISelectedListener<UserEncounterSelectedEventArgs> UserEncounterSelector { get; set; }
        protected ISelector<UserSectionSelectedEventArgs> UserSectionSelector { get; set; }
        protected ISelector<UserTabSelectedEventArgs> UserTabSelector { get; set; }
        protected AnimationMonitor AnimationMonitor { get; set; }
        protected SwipeManager SwipeManager { get; set; }
        protected IShiftTransformsAnimator Curve { get; set; }
        [Inject]
        public virtual void Inject(
            AnimationMonitor animationMonitor,
            IShiftTransformsAnimator curve,
            SwipeManager swipeManager,
            ISelectedListener<UserEncounterSelectedEventArgs> userEncounterSelector,
            ISelector<UserSectionSelectedEventArgs> userSectionSelector,
            ISelector<UserTabSelectedEventArgs> userTabSelector)
        {
            UserEncounterSelector = userEncounterSelector;
            UserSectionSelector = userSectionSelector;
            UserTabSelector = userTabSelector;

            AnimationMonitor = animationMonitor;
            Curve = curve;
            SwipeManager = swipeManager;
        }

        protected virtual void Start()
        {
            UserEncounterSelector.Selected += OnEncounterSelected;
            if (UserEncounterSelector.CurrentValue != null)
                OnEncounterSelected(UserEncounterSelector, UserEncounterSelector.CurrentValue);

            UserSectionSelector.Selected += OnSectionSelected;
            if (UserSectionSelector.CurrentValue != null)
                OnSectionSelected(UserSectionSelector, UserSectionSelector.CurrentValue);

            UserTabSelector.Selected += OnTabSelected;
            if (UserTabSelector.CurrentValue != null)
                OnTabSelected(UserTabSelector, UserTabSelector.CurrentValue);
        }

        protected ReaderSectionContent Current { get; set; }
        protected ReaderSectionContent Next { get; set; }
        protected ReaderSectionContent Previous { get; set; }
        protected ReaderSectionContent Leaving { get; set; }

        protected virtual OrderedCollection<UserSection> Sections { get; set; }
        protected virtual void OnEncounterSelected(object sender, UserEncounterSelectedEventArgs eventArgs)
        {
            Sections = eventArgs.Encounter.Sections;
            ClearCurrent();
        }

        protected virtual void OnEnable() => ClearCurrent();

        protected virtual void ClearCurrent()
        {
            Leaving = Current;
            Current = null;
            if (Leaving == null)
                return;

            Leaving.UserSectionSelected -= UserSectionSelector.Select;
            Leaving.UserTabSelected -= UserTabSelector.Select;
            Leaving.Select(this, new UserTabSelectedEventArgs(Leaving.Tab, ChangeType.Inactive));
        }

        protected virtual void OnSectionSelected(object sender, UserSectionSelectedEventArgs eventArgs)
        {
            if (Current != null && Current.Section == eventArgs.SelectedSection)
                return;

            if (eventArgs.ChangeType == ChangeType.JumpTo || eventArgs.ChangeType == ChangeType.Inactive)
                ClearCurrent();

            HandleSectionChange(sender, eventArgs);
        }

        protected virtual void OnTabSelected(object sender, UserTabSelectedEventArgs eventArgs)
        {
            Current.UserTabSelected -= UserTabSelector.Select;
            Current.Select(sender, eventArgs);
            Current.UserTabSelected += UserTabSelector.Select;
        }

        protected UserSection PreviousSection { get; set; }
        protected UserSection NextSection { get; set; }
        protected virtual void HandleSectionChange(object sender, UserSectionSelectedEventArgs eventArgs)
        {
            var sectionIndex = Sections.IndexOf(eventArgs.SelectedSection);
            PreviousSection = (sectionIndex > 0) ? Sections[sectionIndex - 1].Value : null;
            NextSection = (sectionIndex < Sections.Count - 1) ? Sections[sectionIndex + 1].Value : null;

            ClearCurrent();
            Previous = null;
            Next = null;

            var unusedContent = new Stack<ReaderSectionContent>();
            foreach (var sectionContent in Contents) {
                if (sectionContent.Section == null)
                    unusedContent.Push(sectionContent);
                else if (sectionContent.Section == PreviousSection)
                    Previous = sectionContent;
                else if (sectionContent.Section == eventArgs.SelectedSection)
                    Current = sectionContent;
                else if (sectionContent.Section == NextSection)
                    Next = sectionContent;
                else if (sectionContent == Leaving)
                    continue;
                else
                    unusedContent.Push(sectionContent);
            }

            if (Current == null)
                Current = unusedContent.Pop();
            if (PreviousSection != null && Previous == null)
                Previous = unusedContent.Pop();
            if (NextSection != null && Next == null)
                Next = unusedContent.Pop();

            Current.Select(sender, eventArgs);
            Current.UserSectionSelected += UserSectionSelector.Select;
            Current.UserTabSelected += UserTabSelector.Select;

            SectionDraw();
        }

        #region Animation
        private Coroutine currentCoroutine;
        protected virtual void SectionDraw()
        {
            foreach (var sectionContent in Contents)
                sectionContent.gameObject.SetActive(sectionContent == Current || sectionContent == Leaving);

            if (currentCoroutine != null) {
                AnimationMonitor.StopCoroutine(currentCoroutine);
                AnimationMonitor.AnimationStopping(this);
                SwipeManager.ReenableSwipe();
            }

            currentCoroutine = AnimationMonitor.StartCoroutine(GetTransition());
        }

        protected virtual IEnumerator GetTransition()
        {
            IEnumerator shiftSectionRoutine = GetShiftRoutine();
            if (shiftSectionRoutine != null)
                return AnimationTransition(shiftSectionRoutine);
            else
                return InstantTransition();
        }

        protected virtual IEnumerator SetNextAndPrevious()
        {
            yield return null;
            while (AnimationMonitor.IsAnimationInProgress())
                yield return null;

            if (Previous != null) {
                Previous.Select(this, new UserSectionSelectedEventArgs(PreviousSection, ChangeType.Inactive));
                Previous.SetLastTab(this, ChangeType.Inactive);
            }

            if (Next != null) {
                Next.Select(this, new UserSectionSelectedEventArgs(NextSection, ChangeType.Inactive));
                Next.SetFirstTab(this, ChangeType.Inactive);
            }
        }

        protected virtual IEnumerator InstantTransition()
        {
            Curve.SetPosition(Current.RectTransform);
            if (Leaving != null)
                Leaving.gameObject.SetActive(false);

            yield return SetNextAndPrevious();
        }

        protected IEnumerator GetShiftRoutine()
        {
            if (!gameObject.activeInHierarchy || Leaving == null)
                return null;
            else if (Sections.IndexOf(Leaving.Section) < Sections.IndexOf(Current.Section))
                return ShiftForward(Leaving);
            else
                return ShiftBackward(Leaving);
        }

        protected IEnumerator ShiftForward(ReaderSectionContent leavingContent)
            => AnimationTransition(Curve.ShiftForward(leavingContent.RectTransform, Current.RectTransform));
        protected IEnumerator ShiftBackward(ReaderSectionContent leavingContent)
            => AnimationTransition(Curve.ShiftBackward(leavingContent.RectTransform, Current.RectTransform));
        protected IEnumerator AnimationTransition(IEnumerator enumerator)
        {
            AnimationMonitor.AnimationStarting(this);
            SwipeManager.DisableSwipe();
            yield return enumerator;

            SwipeManager.ReenableSwipe();
            AnimationMonitor.AnimationStopping(this);

            yield return SetNextAndPrevious();
        }
        #endregion

        #region Swipe
        public void SwipeStart()
        {
            CanvasGroup.blocksRaycasts = false;

            if (currentCoroutine != null) {
                StopCoroutine(currentCoroutine);
                currentCoroutine = null;
            }
        }
        public void SwipeUpdate(Direction dir, float dist)
        {
            if (dir == Direction.NA) {
                if (Next != null)
                    Next.gameObject.SetActive(false);
                if (Previous != null)
                    Previous.gameObject.SetActive(false);
                Curve.SetPosition(Current.RectTransform);
                return;
            }

            ReaderSectionContent hiddenContent = (dir == Direction.Right) ? Next : Previous;
            ReaderSectionContent shownContent = (dir == Direction.Right) ? Previous : Next;
            if (hiddenContent != null)
                hiddenContent.gameObject.SetActive(false);
            shownContent.gameObject.SetActive(true);
            if (dir == Direction.Right)
                Curve.SetMoveAmountBackward(Current.RectTransform, shownContent.RectTransform, dist);
            else
                Curve.SetMoveAmountForward(Current.RectTransform, shownContent.RectTransform, dist);
        }

        public void SwipeEnd(Direction dir, float dist, bool changingSections)
        {
            CanvasGroup.blocksRaycasts = true;
            if (changingSections)
                return;

            if (dir == Direction.Right && dist > 0)
                currentCoroutine = StartCoroutine(ShiftForward(Previous));
            else if (dir == Direction.Left && dist < 0)
                currentCoroutine = StartCoroutine(ShiftBackward(Next));
        }
        #endregion
    }
}

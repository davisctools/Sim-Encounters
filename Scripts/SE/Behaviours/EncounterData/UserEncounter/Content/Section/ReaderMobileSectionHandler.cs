using ClinicalTools.Collections;
using ClinicalTools.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ReaderMobileSectionHandler : MonoBehaviour
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

        protected ISelector<UserEncounterSelectedEventArgs> UserEncounterSelector { get; set; }
        protected ISelector<UserSectionSelectedEventArgs> UserSectionSelector { get; set; }
        protected ISelector<UserTabSelectedEventArgs> UserTabSelector { get; set; }
        protected SwipeManager SwipeManager { get; set; }
        protected IShiftTransformsAnimator Curve { get; set; }
        [Inject]
        public virtual void Inject(
            IShiftTransformsAnimator curve,
            SwipeManager swipeManager,
            ISelector<UserEncounterSelectedEventArgs> userEncounterSelector,
            ISelector<UserSectionSelectedEventArgs> userSectionSelector,
            ISelector<UserTabSelectedEventArgs> userTabSelector)
        {
            Curve = curve;
            SwipeManager = swipeManager;

            UserEncounterSelector = userEncounterSelector;
            UserSectionSelector = userSectionSelector;
            UserTabSelector = userTabSelector;
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

        protected virtual void OnEnable()
        {
            if (SwipeParamater == null)
                InitializeSwipeParamaters();
            SwipeManager.AddSwipeAction(SwipeParamater);

            ClearCurrent();
        }
        protected virtual void OnDisable() => SwipeManager.RemoveSwipeAction(SwipeParamater);

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

            HandleSectionStuff(sender, eventArgs);
        }

        protected virtual void OnTabSelected(object sender, UserTabSelectedEventArgs eventArgs)
        {
            Current.UserTabSelected -= UserTabSelector.Select;
            Current.Select(sender, eventArgs);
            Current.UserTabSelected += UserTabSelector.Select;
        }

        protected UserSection PreviousSection { get; set; }
        protected UserSection NextSection { get; set; }
        protected virtual void HandleSectionStuff(object sender, UserSectionSelectedEventArgs eventArgs)
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

            TabDraw();
        }

        private Coroutine currentCoroutine;
        protected virtual void TabDraw()
        {
            foreach (var sectionContent in Contents)
                sectionContent.gameObject.SetActive(sectionContent == Current || sectionContent == Leaving);

            if (currentCoroutine != null) {
                StopCoroutine(currentCoroutine);
                SwipeManager.ReenableSwipe();
            }

            IEnumerator shiftSectionRoutine = GetShiftRoutine();
            if (shiftSectionRoutine != null) {
                currentCoroutine = StartCoroutine(shiftSectionRoutine);
            } else {
                Curve.SetPosition(Current.RectTransform);
                SetNextAndPrevious();
            }
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
        protected virtual void SetNextAndPrevious()
        {
            if (Previous != null) {
                Previous.Select(this, new UserSectionSelectedEventArgs(PreviousSection, ChangeType.Inactive));
                Previous.SetLastTab(this, ChangeType.Inactive);
            }

            if (Next != null) {
                Next.Select(this, new UserSectionSelectedEventArgs(NextSection, ChangeType.Inactive));
                Next.SetFirstTab(this, ChangeType.Inactive);
            }
        }

        protected IEnumerator ShiftForward(ReaderSectionContent leavingContent)
            => Shift(Curve.ShiftForward(leavingContent.RectTransform, Current.RectTransform));
        protected IEnumerator ShiftBackward(ReaderSectionContent leavingContent)
            => Shift(Curve.ShiftBackward(leavingContent.RectTransform, Current.RectTransform));
        protected IEnumerator Shift(IEnumerator enumerator)
        {
            SwipeManager.DisableSwipe();
            yield return enumerator;
            SwipeManager.ReenableSwipe();
            yield return null;
            SetNextAndPrevious();
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

        private void SwipeStart(Swipe obj)
        {
            DragOverrideScript.DragAllowed = false;
            CanvasGroup.blocksRaycasts = false;
            SwipeUpdate(obj);
        }
        private void SwipeUpdate(Swipe obj)
        {
            var dist = (obj.LastPosition.x - obj.StartPosition.x) / Screen.width;
            if (dist > 0)
                RightSwipeUpdate(Mathf.Clamp01(dist));
            else
                LeftSwipeUpdate(Mathf.Clamp01(-dist));
        }

        private bool swipingLeft, swipingRight;
        private void RightSwipeUpdate(float dist)
        {
            if (Next != null)
                Next.gameObject.SetActive(false);
            if (Previous == null || Current.Section.Data.CurrentTabIndex != 0) {
                Curve.SetPosition(Current.RectTransform);
                return;
            }
            swipingRight = true;
            Previous.gameObject.SetActive(true);
            Curve.SetMoveAmountBackward(Current.RectTransform, Previous.RectTransform, dist);
        }
        private void LeftSwipeUpdate(float dist)
        {
            if (Previous != null)
                Previous.gameObject.SetActive(false);
            if (Next == null || Current.Section.Data.CurrentTabIndex + 1 != Current.Section.Tabs.Count) {
                Curve.SetPosition(Current.RectTransform);
                return;
            }
            swipingLeft = true;
            Next.gameObject.SetActive(true);
            Curve.SetMoveAmountForward(Current.RectTransform, Next.RectTransform, dist);
        }

        private void SwipeEnd(Swipe obj)
        {
            DragOverrideScript.DragAllowed = true;
            CanvasGroup.blocksRaycasts = true;

            var dist = (obj.LastPosition.x - obj.StartPosition.x) / Screen.width;
            if (swipingRight && dist > 0 && Previous != null && Current.Section.Data.CurrentTabIndex == 0) {
                if (dist > .5f || obj.Velocity.x / Screen.dpi > 1.5f) {
                    Previous.Section.Data.CurrentTabIndex = Previous.Section.Data.Tabs.Count - 1;
                    UserSectionSelector.Select(this, new UserSectionSelectedEventArgs(Previous.Section, ChangeType.Previous));
                } else {
                    currentCoroutine = StartCoroutine(ShiftForward(Previous));
                }
            } else if (swipingLeft && dist < 0 && Next != null && Current.Section.Data.CurrentTabIndex + 1 == Current.Section.Tabs.Count) {
                if (dist < -.5f || obj.Velocity.x / Screen.dpi < -1.5f)
                    UserSectionSelector.Select(this, new UserSectionSelectedEventArgs(Next.Section, ChangeType.Next));
                else
                    currentCoroutine = StartCoroutine(ShiftBackward(Next));
            }

            swipingLeft = false;
            swipingRight = false;
        }
    }
}

using ClinicalTools.Collections;
using ClinicalTools.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using static ClinicalTools.SimEncounters.ReaderGeneralSectionHandler;

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
        protected IShiftTransformsAnimator Curve { get; set; }
        [Inject]
        public virtual void Inject(
            AnimationMonitor animationMonitor,
            IShiftTransformsAnimator curve,
            ISelectedListener<UserEncounterSelectedEventArgs> userEncounterSelector,
            ISelector<UserSectionSelectedEventArgs> userSectionSelector,
            ISelector<UserTabSelectedEventArgs> userTabSelector)
        {
            UserEncounterSelector = userEncounterSelector;
            UserSectionSelector = userSectionSelector;
            UserTabSelector = userTabSelector;

            AnimationMonitor = animationMonitor;
            Curve = curve;
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

            ResetMovement();

            if (currentCoroutine != null)
                StopCoroutine(currentCoroutine);
            currentCoroutine = StartCoroutine(SetNextAndPrevious());
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

        #endregion

        #region Swipe}
        public void StartMove()
        {
            CanvasGroup.blocksRaycasts = false;

            if (currentCoroutine != null) {
                StopCoroutine(currentCoroutine);
                currentCoroutine = null;
            }
        }
        public void Move(Direction dir, float dist)
        {
            if (Current == null || Current.Section != UserSectionSelector.CurrentValue.SelectedSection)
                OnSectionSelected(UserSectionSelector, UserSectionSelector.CurrentValue);

            if (dir == Direction.NA)
                ResetMovement();
            else
                MoveDirection(dir == Direction.Previous, dist);
        }

        protected virtual void ResetMovement()
        {
            if (Next != null)
                Next.gameObject.SetActive(false);
            if (Previous != null)
                Previous.gameObject.SetActive(false);
            Curve.SetPosition(Current.RectTransform);
        }
        protected virtual void MoveDirection(bool movingRight, float dist)
        {
            ReaderSectionContent hiddenContent = (movingRight) ? Next : Previous;
            ReaderSectionContent shownContent = (movingRight) ? Previous : Next;
            if (hiddenContent != null)
                hiddenContent.gameObject.SetActive(false);
            shownContent.gameObject.SetActive(true);
            if (movingRight)
                Curve.SetMoveAmountBackward(Current.RectTransform, shownContent.RectTransform, dist);
            else
                Curve.SetMoveAmountForward(Current.RectTransform, shownContent.RectTransform, dist);
        }

        public void EndMove()
        {
            CanvasGroup.blocksRaycasts = true;
            ResetMovement();
        }
        #endregion
    }
}

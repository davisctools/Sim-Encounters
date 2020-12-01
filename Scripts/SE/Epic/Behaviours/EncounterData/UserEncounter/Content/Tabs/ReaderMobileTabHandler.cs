﻿using ClinicalTools.Collections;
using ClinicalTools.UI;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ReaderMobileTabHandler : MonoBehaviour
    {
        public ReaderTabContent TabDrawer1 { get => tabContent1; set => tabContent1 = value; }
        [SerializeField] private ReaderTabContent tabContent1;
        public ReaderTabContent TabDrawer2 { get => tabDrawer2; set => tabDrawer2 = value; }
        [SerializeField] private ReaderTabContent tabDrawer2;
        public ReaderTabContent TabDrawer3 { get => tabDrawer3; set => tabDrawer3 = value; }
        [SerializeField] private ReaderTabContent tabDrawer3;
        public ReaderTabContent TabDrawer4 { get => tabDrawer4; set => tabDrawer4 = value; }
        [SerializeField] private ReaderTabContent tabDrawer4;

        protected ReaderTabContent[] Contents { get; } = new ReaderTabContent[4];

        protected virtual void Awake()
        {
            Contents[0] = TabDrawer1;
            Contents[1] = TabDrawer2;
            Contents[2] = TabDrawer3;
            Contents[3] = TabDrawer4;
        }

        protected ISelector<UserSectionSelectedEventArgs> UserSectionSelector { get; set; }
        protected ISelector<UserTabSelectedEventArgs> UserTabSelector { get; set; }
        protected SwipeManager SwipeManager { get; set; }
        protected IShiftTransformsAnimator Curve { get; set; }
        [Inject]
        public virtual void Inject(
            IShiftTransformsAnimator curve,
            SwipeManager swipeManager,
            ISelector<UserSectionSelectedEventArgs> userSectionSelector,
            ISelector<UserTabSelectedEventArgs> userTabSelector)
        {
            UserSectionSelector = userSectionSelector;
            UserTabSelector = userTabSelector;
            Curve = curve;
            SwipeManager = swipeManager;
        }
        protected virtual void Start()
        {
            UserSectionSelector.Selected += OnSectionSelected;
            if (UserSectionSelector.CurrentValue != null)
                OnSectionSelected(UserSectionSelector, UserSectionSelector.CurrentValue);

            UserTabSelector.Selected += OnTabSelected;
            if (UserTabSelector.CurrentValue != null)
                OnTabSelected(UserTabSelector, UserTabSelector.CurrentValue);
        }

        protected ReaderTabContent Current { get; set; }
        protected ReaderTabContent Next { get; set; }
        protected ReaderTabContent Previous { get; set; }
        protected ReaderTabContent Leaving { get; set; }

        protected virtual OrderedCollection<UserTab> Tabs { get; set; }
        protected virtual void OnSectionSelected(object sender, UserSectionSelectedEventArgs eventArgs)
        {
            Tabs = eventArgs.SelectedSection.Tabs;
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
        }

        protected virtual void OnTabSelected(object sender, UserTabSelectedEventArgs eventArgs)
        {
            if (Current != null && Current.Tab == eventArgs.SelectedTab)
                return;

            var stopwatch = Stopwatch.StartNew();
            HandleTabStuff(sender, eventArgs);
            UnityEngine.Debug.LogWarning($"C. {eventArgs.SelectedTab.Data.Name} TAB: {stopwatch.ElapsedMilliseconds}");
        }

        protected UserTab PreviousTab { get; set; }
        protected UserTab NextTab { get; set; }
        protected virtual void HandleTabStuff(object sender, UserTabSelectedEventArgs eventArgs)
        {
            var tabIndex = Tabs.IndexOf(eventArgs.SelectedTab);
            PreviousTab = (tabIndex > 0) ? Tabs[tabIndex - 1].Value : null;
            NextTab = (tabIndex < Tabs.Count - 1) ? Tabs[tabIndex + 1].Value : null;

            ClearCurrent();
            Previous = null;
            Next = null;

            var unusedContent = new Stack<ReaderTabContent>();
            foreach (var tabContent in Contents) {
                if (tabContent.Tab == null)
                    unusedContent.Push(tabContent);
                else if (tabContent.Tab == PreviousTab)
                    Previous = tabContent;
                else if (tabContent.Tab == eventArgs.SelectedTab)
                    Current = tabContent;
                else if (tabContent.Tab == NextTab)
                    Next = tabContent;
                else if (tabContent == Leaving)
                    continue;
                else
                    unusedContent.Push(tabContent);
            }

            if (Current == null)
                Current = unusedContent.Pop();
            if (PreviousTab != null && Previous == null)
                Previous = unusedContent.Pop();
            if (NextTab != null && Next == null)
                Next = unusedContent.Pop();

            var useDelay = Current.Tab != eventArgs.SelectedTab;
            Current.Select(sender, eventArgs);

            TabDraw(useDelay);
        }

        private Coroutine currentCoroutine;
        protected virtual void TabDraw(bool useDelay)
        {
            foreach (var tabContent in Contents)
                tabContent.gameObject.SetActive(tabContent == Current || tabContent == Leaving);

            if (currentCoroutine != null) {
                StopCoroutine(currentCoroutine);
                SwipeManager.ReenableSwipe();
            }

            IEnumerator shiftSectionRoutine = GetShiftRoutine();
            if (shiftSectionRoutine != null) {
                currentCoroutine = StartCoroutine(AnimationCoroutine(shiftSectionRoutine, useDelay));
            } else {
                Curve.SetPosition(Current.RectTransform);
                SetNextAndPrevious();
            }
        }

        protected virtual void SetNextAndPrevious()
        {
            if (Previous != null)
                Previous.Select(this, new UserTabSelectedEventArgs(PreviousTab, ChangeType.Inactive));
            if (Next != null)
                Next.Select(this, new UserTabSelectedEventArgs(NextTab, ChangeType.Inactive));
        }
        protected IEnumerator GetShiftRoutine()
        {
            if (!gameObject.activeInHierarchy || Leaving == null)
                return null;
            else if (Tabs.IndexOf(Leaving.Tab) < Tabs.IndexOf(Current.Tab))
                return ShiftForward(Leaving);
            else
                return ShiftBackward(Leaving);
        }

        protected IEnumerator ShiftForward(ReaderTabContent leavingContent)
            => Curve.ShiftForward(leavingContent.RectTransform, Current.RectTransform);
        protected IEnumerator ShiftBackward(ReaderTabContent leavingContent)
            => Curve.ShiftBackward(leavingContent.RectTransform, Current.RectTransform);
        protected virtual IEnumerator AnimationCoroutine(IEnumerator shiftRoutine, bool useDelay)
        {
            SwipeManager.DisableSwipe();

            if (useDelay) {
                Current.RectTransform.anchorMin = new Vector2(1, Current.RectTransform.anchorMin.y);
                Current.RectTransform.anchorMax = new Vector2(2, Current.RectTransform.anchorMax.y);
                yield return null;
                yield return null;
            }

            yield return shiftRoutine;
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

        private void SwipeStart(Swipe obj) => SwipeUpdate(obj);
        private void SwipeUpdate(Swipe obj)
        {
            var dist = (obj.LastPosition.x - obj.StartPosition.x) / Screen.width;
            if (dist > 0)
                RightSwipeUpdate(Mathf.Clamp01(dist));
            else
                LeftSwipeUpdate(Mathf.Clamp01(-dist));
        }

        private void RightSwipeUpdate(float dist)
        {
            if (Next != null)
                Next.gameObject.SetActive(false);
            if (Previous == null)
                return;
            Previous.gameObject.SetActive(true);
            Curve.SetMoveAmountBackward(Current.RectTransform, Previous.RectTransform, dist);
        }
        private void LeftSwipeUpdate(float dist)
        {
            if (Previous != null)
                Previous.gameObject.SetActive(false);
            if (Next == null)
                return;
            Next.gameObject.SetActive(true);
            Curve.SetMoveAmountForward(Current.RectTransform, Next.RectTransform, dist);
        }
        private void SwipeEnd(Swipe obj)
        {
            SwipeUpdate(obj);

            var dist = (obj.LastPosition.x - obj.StartPosition.x) / Screen.width;
            if (dist > 0 && Previous != null) {
                if (dist > .5f || obj.Velocity.x / Screen.dpi > 1.5f)
                    UserTabSelector.Select(this, new UserTabSelectedEventArgs(Previous.Tab, ChangeType.Previous));
                else
                    currentCoroutine = StartCoroutine(ShiftForward(Previous));
            } else if (dist < 0 && Next != null) {
                if (dist < -.5f || obj.Velocity.x / Screen.dpi < -1.5f)
                    UserTabSelector.Select(this, new UserTabSelectedEventArgs(Next.Tab, ChangeType.Next));
                else
                    currentCoroutine = StartCoroutine(ShiftBackward(Next));
            }
        }
    }
}

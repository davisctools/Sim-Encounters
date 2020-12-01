﻿using ClinicalTools.UI;
using System.Collections;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ReaderSidebarController : MonoBehaviour, ISidebarController
    {
        public GameObject Sidebar { get => sidebar; set => sidebar = value; }
        [SerializeField] private GameObject sidebar;
        public CanvasGroup SidebarMainPanel { get => sidebarMainPanel; set => sidebarMainPanel = value; }
        [SerializeField] private CanvasGroup sidebarMainPanel;
        public CanvasGroup SidebarDimBackground { get => sidebarDimBackground; set => sidebarDimBackground = value; }
        [SerializeField] private CanvasGroup sidebarDimBackground;

        protected AndroidBackButton BackButton { get; set; }
        private SwipeManager swipeManager;
        [Inject]
        public virtual void Inject(AndroidBackButton backButton, SwipeManager swipeManager)
        {
            BackButton = backButton;
            this.swipeManager = swipeManager;
        }
        protected virtual void Awake()
        {
            InitializeSidebarParamaters();
        }

        public virtual void Open() => StartCoroutine(OpenSidebarEnumerator());
        public virtual void Close() => StartCoroutine(CloseSidebarEnumerator());

        SwipeParameter OpenSidebarSwipeParamater, CloseSidebarSwipeParamater;
        protected virtual void InitializeSidebarParamaters()
        {
            OpenSidebarSwipeParamater = new SwipeParameter {
                StartPositionRange = new Rect(0, 0, Screen.width / 3, 10000)
            };
            OpenSidebarSwipeParamater.AngleRanges.Add(new AngleRange(-30, 30));
            OpenSidebarSwipeParamater.OnSwipeStart += OpenSwipeStart;
            OpenSidebarSwipeParamater.OnSwipeUpdate += OpenSwipeUpdate;
            OpenSidebarSwipeParamater.OnSwipeEnd += OpenSwipeEnd;

            CloseSidebarSwipeParamater = new SwipeParameter {
                StartPositionRange = new Rect(0, 0, 10000, 10000)
            };
            CloseSidebarSwipeParamater.AngleRanges.Add(new AngleRange(150, 210));
            CloseSidebarSwipeParamater.OnSwipeStart += CloseSwipeStart;
            CloseSidebarSwipeParamater.OnSwipeUpdate += CloseSwipeUpdate;
            CloseSidebarSwipeParamater.OnSwipeEnd += CloseSwipeEnd;

            //swipeManager.AddSwipeAction(OpenSidebarSwipeParamater);
        }

        protected virtual void OpenSwipeStart(Swipe swipe)
        {
            BeginShowingSidebar();
            OpenSwipeUpdate(swipe);
        }
        protected virtual void OpenSwipeUpdate(Swipe swipe)
            => SetSidebar(GetDistance(swipe) / .8f);
        protected virtual void OpenSwipeEnd(Swipe swipe)
        {
            if (GetDistance(swipe) > .4f)
                StartCoroutine(OpenSidebarEnumerator());
            else
                StartCoroutine(CloseSidebarEnumerator());
        }


        protected virtual void CloseSwipeStart(Swipe swipe)
        {
            BeginHidingSidebar();
            CloseSwipeUpdate(swipe);
        }
        protected virtual void CloseSwipeUpdate(Swipe swipe)
            => SetSidebar(1 + (GetDistance(swipe) / .8f));
        protected virtual void CloseSwipeEnd(Swipe swipe)
        {
            if (GetDistance(swipe) < -.4f)
                StartCoroutine(CloseSidebarEnumerator());
            else
                StartCoroutine(OpenSidebarEnumerator());
        }


        protected virtual float GetDistance(Swipe swipe)
            => (swipe.LastPosition.x - swipe.StartPosition.x) / Screen.width;


        protected virtual void SetSidebar(float proportionOfAnimation)
        {
            proportionOfAnimation = Mathf.Clamp01(proportionOfAnimation);

            var x = proportionOfAnimation * .8f;
            var alpha = proportionOfAnimation;
            sidebarDimBackground.alpha = alpha;

            var rectTransform = (RectTransform)SidebarMainPanel.transform;
            var anchorMin = rectTransform.anchorMin;
            anchorMin.x = x - .8f;
            rectTransform.anchorMin = anchorMin;
            var anchorMax = rectTransform.anchorMax;
            anchorMax.x = x;
            rectTransform.anchorMax = anchorMax;
        }

        protected void BeginShowingSidebar()
            => Sidebar.SetActive(true);
        protected void CompleteShowingSidebar()
        {
            SidebarMainPanel.interactable = true;
            SidebarDimBackground.interactable = true;
            swipeManager.AddSwipeAction(CloseSidebarSwipeParamater);
            //swipeManager.RemoveSwipeAction(OpenSidebarSwipeParamater);

            BackButton.Register(StartCloseEnumerator);
        }


        protected void BeginHidingSidebar()
        {
            SidebarMainPanel.interactable = false;
            SidebarDimBackground.interactable = false;
            BackButton.Deregister(StartCloseEnumerator);
        }
        protected void CompleteHidingSidebar()
        {
            Sidebar.SetActive(false);
            //swipeManager.AddSwipeAction(OpenSidebarSwipeParamater);
            swipeManager.RemoveSwipeAction(CloseSidebarSwipeParamater);
        }


        private const float OpenTime = .3f;
        public IEnumerator OpenSidebarEnumerator()
        {
            BeginShowingSidebar();

            var proportionOfAnimation = sidebarDimBackground.alpha;
            while (proportionOfAnimation < 1) {
                proportionOfAnimation += Time.deltaTime / OpenTime;
                SetSidebar(proportionOfAnimation);
                yield return null;
            }
            SetSidebar(1);
            CompleteShowingSidebar();
        }

        private const float HideTime = .3f;
        protected void StartCloseEnumerator() => StartCoroutine(CloseSidebarEnumerator());
        public IEnumerator CloseSidebarEnumerator()
        {
            BeginHidingSidebar();

            var proportionOfAnimation = sidebarDimBackground.alpha;
            while (proportionOfAnimation > 0) {
                proportionOfAnimation -= Time.deltaTime / HideTime;
                SetSidebar(proportionOfAnimation);
                yield return null;
            }
            SetSidebar(0);
            CompleteHidingSidebar();
        }
    }
}
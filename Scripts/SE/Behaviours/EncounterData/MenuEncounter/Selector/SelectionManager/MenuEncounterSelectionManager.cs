using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class MenuEncounterSelectionManager : BaseMenuEncounterSelectionManager
    {
        public SidebarUI Sidebar { get => sidebar; set => sidebar = value; }
        [SerializeField] private SidebarUI sidebar;
        public List<BaseViewEncounterSelector> EncounterViews { get => encounterViews; set => encounterViews = value; }
        [SerializeField] private List<BaseViewEncounterSelector> encounterViews;
        public ToggleViewButtonUI ToggleViewButton { get => toggleViewButton; set => toggleViewButton = value; }
        [SerializeField] private ToggleViewButtonUI toggleViewButton;
        public ScrollRect ScrollRect { get => scrollRect; set => scrollRect = value; }
        [SerializeField] private ScrollRect scrollRect;

        private int currentViewIndex = 0;

        protected EncounterFilterBehaviour Filters { get; set; }
        [Inject] public virtual void Inject([InjectOptional] EncounterFilterBehaviour filters) => Filters = filters;


        protected void Awake()
        {
            if (ToggleViewButton != null) {
                ToggleViewButton.Display(GetNextView());
                ToggleViewButton.Selected += ChangeView;
            }
            if (Sidebar != null) {
                Sidebar.SearchStuff.SortingOrder.SortingOrderChanged += (sortingOrder) => ShowEncounters();
                Sidebar.SearchStuff.Filters.FilterChanged += (filter) => ShowEncounters();
            }

            if (Filters != null)
                Filters.FilterChanged += (filter) => ShowEncounters();
        }

        public override void Initialize()
        {
            EncounterViews[currentViewIndex].Hide();
            currentViewIndex = 0;
        }

        protected MenuSceneInfo SceneInfo { get; set; }
        protected IEnumerable<MenuEncounter> CurrentEncounters { get; set; }
        protected bool IsRead { get; set; }
        public override void DisplayForRead(MenuSceneInfo sceneInfo, IEnumerable<MenuEncounter> encounters)
        {
            IsRead = true;
            Display(sceneInfo, encounters);
        }

        public override void DisplayForEdit(MenuSceneInfo sceneInfo, IEnumerable<MenuEncounter> encounters)
        {
            IsRead = false;
            Display(sceneInfo, encounters);
        }

        protected virtual void Display(MenuSceneInfo sceneInfo, IEnumerable<MenuEncounter> encounters)
        {
            SceneInfo = sceneInfo;
            CurrentEncounters = encounters;

            ShowEncounters();
            if (Sidebar != null)
                Sidebar.Show();
            if (ToggleViewButton != null)
                ToggleViewButton.Show();
        }

        private IEnumerable<MenuEncounter> FilterEncounterDetails(IEnumerable<MenuEncounter> encounters)
        {
            if (Filters == null)
                return encounters;

            var filter = Filters.EncounterFilter;
            return encounters.Where(e => filter(e));
        }

        private void ShowEncounters()
        {
            var encounters = new List<MenuEncounter>(FilterEncounterDetails(CurrentEncounters));
            if (Sidebar != null)
                encounters.Sort(Sidebar.SearchStuff.SortingOrder.Comparison);

            var encounterView = EncounterViews[currentViewIndex];
            if (IsRead)
                encounterView.DisplayForRead(SceneInfo, encounters);
            else
                encounterView.DisplayForEdit(SceneInfo, encounters);

            //ScrollRect.content = (RectTransform)encounterView.transform;
            if (ScrollRect.content != null)
                ScrollRect.verticalNormalizedPosition = 1;
        }

        protected void ChangeView()
        {
            EncounterViews[currentViewIndex].Hide();

            currentViewIndex++;
            if (currentViewIndex >= EncounterViews.Count)
                currentViewIndex = 0;

            if (ToggleViewButton != null)
                ToggleViewButton.Display(GetNextView());
            ShowEncounters();
        }

        protected BaseViewEncounterSelector GetNextView()
        {
            var viewIndex = (currentViewIndex + 1) % EncounterViews.Count;
            return EncounterViews[viewIndex];
        }

        public override void Hide()
        {
            EncounterViews[currentViewIndex].Hide();
            if (ToggleViewButton != null)
                ToggleViewButton.Hide();

            if (Sidebar != null)
                Sidebar.Hide();
        }
    }
}
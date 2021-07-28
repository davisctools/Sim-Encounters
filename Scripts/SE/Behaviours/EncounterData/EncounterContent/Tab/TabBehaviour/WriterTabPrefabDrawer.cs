using ClinicalTools.Collections;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class WriterTabPrefabDrawer : MonoBehaviour
    {
        public Transform TabParent { get => tabParent; set => tabParent = value; }
        [SerializeField] private Transform tabParent;

        protected ISelectedListener<SectionSelectedEventArgs> SectionSelectedListener { get; set; }
        protected ISelectedListener<TabSelectedEventArgs> TabSelectedListener { get; set; }
        protected BaseTabDrawer.Factory TabDrawerFactory { get; set; }
        [Inject]
        public virtual void Inject(
            SignalBus signalBus,
            ISelectedListener<SectionSelectedEventArgs> sectionSelectedListener ,
            ISelectedListener<TabSelectedEventArgs> tabSelectedListener, 
            BaseTabDrawer.Factory tabDrawerFactory)
        {
            signalBus.Subscribe<SerializeEncounterSignal>(Serialize);
            SectionSelectedListener = sectionSelectedListener;
            TabDrawerFactory = tabDrawerFactory;
            TabSelectedListener = tabSelectedListener;
            TabSelectedListener.Selected += OnTabSelected;
        }
        protected OrderedCollection<Tab> TabCollection { get; set; }
        protected string TabKey { get; set; }
        protected Tab CurrentTab { get; set; }
        protected BaseTabDrawer CurrentTabDrawer { get; set; }
        public virtual void OnTabSelected(object sender, TabSelectedEventArgs e)
        {
            if (e.SelectedTab == CurrentTab)
                return;
            CurrentTab = e.SelectedTab;

            if (CurrentTabDrawer != null) {
                Serialize();
                Destroy(CurrentTabDrawer.gameObject);
            }

            if (CurrentTab == null) {
                CurrentTabDrawer = null;
                return;
            }

            TabCollection = SectionSelectedListener.CurrentValue.SelectedSection.Tabs;
            TabKey = TabCollection[CurrentTab];

            CurrentTabDrawer = TabDrawerFactory.Create(GetTabPath(CurrentTab));
            CurrentTabDrawer.transform.SetParent(TabParent);
            CurrentTabDrawer.transform.localScale = Vector3.one;
            ((RectTransform)CurrentTabDrawer.transform).offsetMin = Vector2.zero;
            ((RectTransform)CurrentTabDrawer.transform).offsetMax = Vector2.zero;

            CurrentTabDrawer.Display(sender, e);
        }

        protected virtual string GetTabPath(Tab tab)
        {
            var tabFolder = $"Prefabs/Desktop/Writer/Tabs/{tab.Type} Tab/";
            return $"{tabFolder}{tab.Type.Replace(" ", string.Empty)}Tab";
        }

        protected virtual void Serialize()
        {
            if (CurrentTabDrawer != null)
                TabCollection[TabKey] = CurrentTabDrawer.Serialize();
        }
    }
}
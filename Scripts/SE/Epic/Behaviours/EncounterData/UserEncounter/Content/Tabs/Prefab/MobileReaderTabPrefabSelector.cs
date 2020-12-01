using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class MobileReaderTabPrefabSelector : MonoBehaviour
    {
        public Transform TabParent { get => tabParent; set => tabParent = value; }
        [SerializeField] private Transform tabParent;

        protected UserTab CurrentTab { get; set; }
        protected ISelectedListener<UserTabSelectedEventArgs> UserTabSelector { get; set; }
        protected UserTabSelectorBehaviour.Factory TabDrawerFactory { get; set; }
        [Inject]
        public virtual void Inject(
            ISelectedListener<UserTabSelectedEventArgs> userTabSelector,
            UserTabSelectorBehaviour.Factory tabDrawerFactory)
        {
            UserTabSelector = userTabSelector;
            TabDrawerFactory = tabDrawerFactory;
        }
        protected virtual void Start()
        {
            UserTabSelector.Selected += OnTabSelected;
            if (UserTabSelector.CurrentValue != null)
                OnTabSelected(UserTabSelector, UserTabSelector.CurrentValue);
        }
        protected virtual void OnDestroy() => UserTabSelector.Selected -= OnTabSelected;

        protected UserTabSelectorBehaviour CurrentTabDrawer { get; set; }
        protected virtual void OnTabSelected(object sender, UserTabSelectedEventArgs eventArgs)
        {
            if (CurrentTab == eventArgs.SelectedTab) {
                if (CurrentTabDrawer != null)
                    CurrentTabDrawer.Select(sender, eventArgs);
                return;
            }

            CurrentTab = eventArgs.SelectedTab;

            if (CurrentTabDrawer != null)
                Destroy(CurrentTabDrawer.gameObject);

            var tab = eventArgs.SelectedTab;
            CurrentTabDrawer = TabDrawerFactory.Create(GetTabPrefabPath(tab.Data));
            CurrentTabDrawer.transform.SetParent(TabParent);
            CurrentTabDrawer.Select(sender, eventArgs);
        }

        protected virtual string GetTabPrefabPath(Tab tab)
        {
            var tabFolder = $"Prefabs/Shared/Reader/Tabs/{tab.Type} Tab/";
            return $"{tabFolder}{tab.Type.Replace(" ", string.Empty)}Tab";
        }
    }
}
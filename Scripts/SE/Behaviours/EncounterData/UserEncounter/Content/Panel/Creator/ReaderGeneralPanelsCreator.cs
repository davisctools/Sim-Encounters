using ClinicalTools.Collections;
using ClinicalTools.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ReaderGeneralPanelsCreator<T> : BaseChildUserPanelsDrawer
        where T : BaseReaderPanelBehaviour
    {
        [SerializeField] private Transform childParent = null;
        public Transform ChildParent {
            get {
                if (childParent != null)
                    return childParent;
                return transform;
            }
        }

        public List<T> PanelOptions { get => panelOptions; set => panelOptions = value; }
        [SerializeField] private List<T> panelOptions = new List<T>();

        public GameObject SpacerPrefab { get => spacerPrefab; set => spacerPrefab = value; }
        [SerializeField] private GameObject spacerPrefab;

        protected Stack<GameObject> SpacerStack { get; } = new Stack<GameObject>();
        protected List<GameObject> Spacers { get; } = new List<GameObject>();

        protected BaseReaderPanelBehaviour.Factory ReaderPanelFactory { get; set; }
        protected GameObjectFactory GameObjectFactory { get; set; }
        [Inject]
        public virtual void Inject(
            BaseReaderPanelBehaviour.Factory readerPanelFactory,
            GameObjectFactory gameObjectFactory)
        {
            ReaderPanelFactory = readerPanelFactory;
            GameObjectFactory = gameObjectFactory;
        }

        protected OrderedCollection<UserPanel> CurrentPanels { get; set; }
        protected bool IsActive { get; set; }
        protected Dictionary<UserPanel, T> Children { get; } = new Dictionary<UserPanel, T>();

        public override void Display(OrderedCollection<UserPanel> panels, bool active)
        {
            if (CurrentPanels == panels) {
                HandleSamePanels(panels, active);
                return;
            }

            CurrentPanels = panels;
            IsActive = active;

            foreach (var child in Children)
                Destroy(child.Value.gameObject);
            Children.Clear();

            foreach (var spacer in Spacers)
                SpacerStack.Push(spacer);

            DrawChildren(panels.ValueArr, active);

            while (SpacerStack.Count > 0)
                SpacerStack.Pop().SetActive(false);
        }

        protected virtual void HandleSamePanels(OrderedCollection<UserPanel> panels, bool active)
        {
            if (IsActive == active)
                return;
            IsActive = active;
            if (IsActive)
                NextFrame.Function(SelectChildren);
        }

        protected virtual void SelectChildren()
        {
            foreach (var child in Children)
                child.Value.Select(this, new UserPanelSelectedEventArgs(child.Key, IsActive));
        }

        protected virtual void DrawChildren(UserPanel[] panelValues, bool active)
        {
            for (int i = 0; i < panelValues.Length; i++) {
                if (i > 0)
                    DrawSpacer();
                DrawPanel(panelValues[i], active);
            }
        }

        protected virtual void DrawSpacer()
        {
            if (SpacerPrefab == null)
                return;

            GameObject spacer;
            if (SpacerStack.Count > 0) {
                spacer = SpacerStack.Pop();
                spacer.SetActive(true);
            } else {
                spacer = GameObjectFactory.Create(SpacerPrefab);
                spacer.transform.SetParent(ChildParent);
                Spacers.Add(spacer);
            }
            spacer.transform.SetAsLastSibling();
        }

        protected virtual T DrawPanel(UserPanel panel, bool active)
        {
            var prefab = GetChildPanelPrefab(panel);
            if (prefab == null)
                return null;

            var panelBehaviour = (T)ReaderPanelFactory.Create(prefab);
            panelBehaviour.transform.SetParent(ChildParent);
            panelBehaviour.transform.localScale = Vector3.one;
            panelBehaviour.Select(this, new UserPanelSelectedEventArgs(panel, active));
            Children.Add(panel, panelBehaviour);

            return panelBehaviour;
        }

        protected virtual T GetChildPanelPrefab(UserPanel childPanel)
        {
            var type = childPanel.Data.Type;

            if (panelOptions.Count == 1)
                return panelOptions[0];

            foreach (var panelOption in panelOptions) {
                if (string.Equals(type, panelOption.Type, StringComparison.OrdinalIgnoreCase))
                    return panelOption;
            }

            Debug.LogError($"No prefab for panel type \"{type}\"");
            return null;
        }
    }
}
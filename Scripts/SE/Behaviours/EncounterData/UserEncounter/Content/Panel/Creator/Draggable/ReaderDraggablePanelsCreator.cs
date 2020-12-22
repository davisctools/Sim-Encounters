using ClinicalTools.UI;
using UnityEngine;
using ClinicalTools.SimEncounters.Extensions;
using System.Collections.Generic;
using System;
using ClinicalTools.SEColors;

namespace ClinicalTools.SimEncounters
{
    public class ReaderDraggablePanelsCreator : ReaderGeneralPanelsCreator<ReaderOrderablePanelBehaviour>
    {
        public BaseRearrangeableGroup DraggableGroupUI { get => draggableGroup; set => draggableGroup = value; }
        [SerializeField] private BaseRearrangeableGroup draggableGroup;
        public GameObject Feedback { get => feedback; set => feedback = value; }
        [SerializeField] private GameObject feedback;

        protected virtual IColorManager ColorManager { get; } = new ColorManager();

        protected virtual void Start() => DraggableGroupUI.Rearranged += Rearranged;

        private void Rearranged(object sender, RearrangedEventArgs2 e)
        {
            var panel = ChildOrder[e.OldIndex];
            ChildOrder.RemoveAt(e.OldIndex);
            ChildOrder.Insert(e.NewIndex, panel);

            SetColors();

            if (HasSamePanelOrder(ChildOrder, CurrentPanels.Values))
                Feedback.SetActive(true);
        }

        protected virtual void SetColors()
        {
            for (var i = 0; i < ChildOrder.Count; i++) {
                var distanceFromCorrectPosition = DistanceFromCorrectPosition(i);
                var panelBehaviour = OrderablePanels[ChildOrder[i]];
                if (distanceFromCorrectPosition == 0)
                    panelBehaviour.SetColor(ColorManager.GetColor(ColorType.Correct));
                else if (distanceFromCorrectPosition == 1)
                    panelBehaviour.SetColor(ColorManager.GetColor(ColorType.PartiallyCorrect));
                else if (distanceFromCorrectPosition > 1)
                    panelBehaviour.SetColor(ColorManager.GetColor(ColorType.Incorrect));
            }
        }

        protected List<UserPanel> ChildOrder { get; set; }
        protected override void DrawChildren(UserPanel[] panelValues, bool active)
        {
            var shuffledPanels = ShufflePanels(panelValues);
            ChildOrder = new List<UserPanel>(shuffledPanels);
            base.DrawChildren(shuffledPanels, active);
        }

        protected Dictionary<UserPanel, ReaderOrderablePanelBehaviour> OrderablePanels { get; } 
            = new Dictionary<UserPanel, ReaderOrderablePanelBehaviour>();

        protected override ReaderOrderablePanelBehaviour DrawPanel(UserPanel panel, bool active)
        {
            var readerPanel = base.DrawPanel(panel, active);
            if (readerPanel != null) { 
                DraggableGroupUI.Add(readerPanel);
                OrderablePanels.Add(panel, readerPanel);
            }
            return readerPanel;
        }

        protected UserPanel[] ShufflePanels(IEnumerable<UserPanel> panels)
        {
            var shuffeledPanels = new List<UserPanel>(panels);
            if (shuffeledPanels.Count > 1) {
                while (HasSamePanelOrder(shuffeledPanels, panels))
                    shuffeledPanels.Shuffle();
            }
            return shuffeledPanels.ToArray();
        }

        protected int DistanceFromCorrectPosition(int currentPosition)
        {
            var panel = ChildOrder[currentPosition];
            var correctOrder = CurrentPanels.ValueArr;
            for (int i = 0; i < correctOrder.Length; i++) {
                if (correctOrder[i] != panel)
                    continue;

                return Math.Abs(i - currentPosition);
            }

            return -1;
        }

        private bool HasSamePanelOrder(List<UserPanel> shuffeledPanels, IEnumerable<UserPanel> childPanels)
        {
            var i = 0;
            foreach (var childPanel in childPanels) {
                if (childPanel != shuffeledPanels[i++])
                    return false;
            }
            return true;
        }
    }
}
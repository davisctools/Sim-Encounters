using ClinicalTools.UI;
using UnityEngine;
using ClinicalTools.SEColors;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

namespace ClinicalTools.SimEncounters
{
    public class ReaderSelfReflectionOptionFeedbackDraggablePanelsCreator
        : ReaderGeneralPanelsCreator<ReaderOrderablePanelBehaviour>
    {
        public BaseRearrangeableGroup DraggableGroupUI { get => draggableGroup; set => draggableGroup = value; }
        [SerializeField] private BaseRearrangeableGroup draggableGroup;
        public GameObject Feedback { get => feedback; set => feedback = value; }
        [SerializeField] private GameObject feedback;
        public TextMeshProUGUI FeedbackText { get => feedbackText; set => feedbackText = value; }
        [SerializeField] private TextMeshProUGUI feedbackText;
        public Button SubmitButton { get => submitButton; set => submitButton = value; }
        [SerializeField] private Button submitButton;

        protected virtual IColorManager ColorManager { get; } = new ColorManager();

        protected virtual void Start() => SubmitButton.onClick.AddListener(Submit);

        private const int NumberOfOptionsToShowFeedbackFor = 3;
        protected virtual void Submit()
        {
            Feedback.SetActive(true);
            foreach (var child in Children.Values)
                child.SetColor(new Color(.21f, .36f, .54f));

            var order = DraggableGroupUI.CurrentOrder;
            if (order.Count == 0)
                return;

            var text = GetText(order[0]);
            for (int i = 1; i < NumberOfOptionsToShowFeedbackFor && i < order.Count; i++)
                text += $"\n\n{GetText(order[i])}";

            FeedbackText.text = text;
        }

        private const string OptionValueKey = "OptionValue";
        private const string FeedbackKey = "FeedbackValue";
        protected virtual string GetText(IDraggable draggable)
        {
            if (draggable == null || !Panels.ContainsKey(draggable))
                return "";

            var values = Panels[draggable].Data.LegacyValues;
            if (!values.ContainsKey(OptionValueKey) || !values.ContainsKey(FeedbackKey))
                return "";

            return $"<b>{values[OptionValueKey]}</b>\n{values[FeedbackKey]}";
        }

        protected Dictionary<IDraggable, UserPanel> Panels { get; set; } = new Dictionary<IDraggable, UserPanel>();
        protected override ReaderOrderablePanelBehaviour DrawPanel(UserPanel panel, bool active)
        {
            var readerPanel = base.DrawPanel(panel, active);
            if (readerPanel != null) { 
                DraggableGroupUI.Add(readerPanel);
                Panels.Add(readerPanel, panel);
            }

            return readerPanel;
        }
    }
}
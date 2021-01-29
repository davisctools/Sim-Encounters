using ClinicalTools.UI;
using UnityEngine;
using ClinicalTools.SEColors;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    public class ReaderSelfReflectionSingleFeedbackDraggablePanelsCreator
        : ReaderGeneralPanelsCreator<ReaderOrderablePanelBehaviour>
    {
        public BaseRearrangeableGroup DraggableGroupUI { get => draggableGroup; set => draggableGroup = value; }
        [SerializeField] private BaseRearrangeableGroup draggableGroup;
        public GameObject Feedback { get => feedback; set => feedback = value; }
        [SerializeField] private GameObject feedback;
        public Button SubmitButton { get => submitButton; set => submitButton = value; }
        [SerializeField] private Button submitButton;

        protected virtual IColorManager ColorManager { get; } = new ColorManager();

        protected virtual void Start() => SubmitButton.onClick.AddListener(Submit);

        protected virtual void Submit()
        {
            Feedback.SetActive(true);
            foreach (var child in Children.Values)
                child.SetColor(new Color(.21f, .36f, .54f));
        }


        protected override ReaderOrderablePanelBehaviour DrawPanel(UserPanel panel, bool active)
        {
            var readerPanel = base.DrawPanel(panel, active);
            if (readerPanel != null)
                DraggableGroupUI.Add(readerPanel);

            return readerPanel;
        }
    }
}
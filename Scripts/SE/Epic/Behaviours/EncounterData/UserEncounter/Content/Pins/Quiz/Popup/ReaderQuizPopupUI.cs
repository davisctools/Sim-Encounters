using ClinicalTools.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    public class ReaderQuizPopupUI : BaseUserQuizPinDrawer
    {
        public List<Button> CloseButtons { get => closeButtons; set => closeButtons = value; }
        [SerializeField] private List<Button> closeButtons = new List<Button>();
        public BaseChildUserPanelsDrawer PanelCreator { get => panelCreator; set => panelCreator = value; }
        [SerializeField] private BaseChildUserPanelsDrawer panelCreator;
        public ScrollRect ScrollRect { get => scrollRect; set => scrollRect = value; }
        [SerializeField] private ScrollRect scrollRect;
        public ScrollRectGradient ScrollGradient { get => scrollGradient; set => scrollGradient = value; }
        [SerializeField] private ScrollRectGradient scrollGradient;

        protected virtual void Awake()
        {
            foreach (var closeButton in CloseButtons)
                closeButton.onClick.AddListener(Hide);
        }

        public override void Display(UserQuizPin quizPin)
        {
            SetPanelsAsRead(quizPin.GetPanels());

            gameObject.SetActive(true);
            PanelCreator.Display(quizPin.Panels, true);

            ScrollRect.normalizedPosition = Vector2.one;
            if (ScrollGradient != null)
                ScrollGradient.ResetGradients();
        }

        protected virtual void Hide()
        {
            gameObject.SetActive(false);
        }

        protected virtual void SetPanelsAsRead(IEnumerable<UserPanel> panels)
        {
            foreach (var panel in panels) {
                var childPanels = new List<UserPanel>(panel.GetChildPanels());
                if (childPanels.Count > 0)
                    SetPanelsAsRead(childPanels);
                else
                    panel.SetRead(true);
            }
        }
    }
}
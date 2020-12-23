using ClinicalTools.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    public class ReaderReadMorePopupUI : BaseUserReadMorePinDrawer
    {
        public BaseChildUserPanelsDrawer PanelCreator { get => panelCreator; set => panelCreator = value; }
        [SerializeField] private BaseChildUserPanelsDrawer panelCreator;
        public ScrollRect ScrollRect { get => scrollRect; set => scrollRect = value; }
        [SerializeField] private ScrollRect scrollRect;
        public ScrollRectGradient ScrollGradient { get => scrollGradient; set => scrollGradient = value; }
        [SerializeField] private ScrollRectGradient scrollGradient;

        public override void Display(UserReadMorePin userPin)
        {
            gameObject.SetActive(true);

            PanelCreator.Display(userPin.Panels, true);

            ScrollRect.normalizedPosition = Vector2.one;
            if (ScrollGradient != null)
                ScrollGradient.ResetGradients();
        }
    }
}
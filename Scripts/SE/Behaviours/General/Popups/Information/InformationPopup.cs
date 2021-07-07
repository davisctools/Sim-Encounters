using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    public class InformationPopup : BaseInformationPopup
    {
        public List<Button> CloseButtons { get => closeButtons; set => closeButtons = value; }
        [SerializeField] private List<Button> closeButtons = new List<Button>();

        protected virtual void Awake()
        {
            foreach (var closeButton in CloseButtons)
                closeButton.onClick.AddListener(Hide);
        }

        public override void ShowInformation() => gameObject.SetActive(true);
        protected virtual void Hide() => gameObject.SetActive(false);
    }
}
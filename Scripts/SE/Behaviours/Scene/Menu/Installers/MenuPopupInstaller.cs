using ClinicalTools.SimEncounters;
using UnityEngine;
using Zenject;

namespace ClinicalTools.ClinicalEncounters
{
    public class MenuPopupInstaller : MonoInstaller
    {
        public BaseInformationPopup InformationPopup { get => informationPopup; set => informationPopup = value; }
        [SerializeField] private BaseInformationPopup informationPopup;
        public BaseInstructionsPopup InstructionsPopup { get => instructionsPopup; set => instructionsPopup = value; }
        [SerializeField] private BaseInstructionsPopup instructionsPopup;

        public override void InstallBindings()
        {
            Container.BindInstance(InformationPopup);
            Container.BindInstance(InstructionsPopup);
        }
    }
}
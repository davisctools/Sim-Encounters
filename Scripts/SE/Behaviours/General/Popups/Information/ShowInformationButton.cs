using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    [RequireComponent(typeof(Button))]
    public class ShowInformationButton : MonoBehaviour
    {
        protected BaseInformationPopup InformationPopup { get; set; }

        [Inject]
        public void Inject([InjectOptional] BaseInformationPopup informationPopup)
        {
            if (informationPopup == null)
                gameObject.SetActive(false);
            else
                InformationPopup = informationPopup;
        }

        protected virtual void Start() => GetComponent<Button>().onClick.AddListener(ShowInstructions);
        public virtual void ShowInstructions() => InformationPopup.ShowInformation();
    }
}
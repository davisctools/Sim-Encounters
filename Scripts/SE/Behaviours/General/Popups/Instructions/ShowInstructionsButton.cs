using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    [RequireComponent(typeof(Button))]
    public class ShowInstructionsButton : MonoBehaviour
    {
        protected BaseInstructionsPopup InstructionsPopup { get; set; }

        [Inject]
        public void Inject([InjectOptional] BaseInstructionsPopup instructionsPopup)
        {
            if (instructionsPopup == null) 
                gameObject.SetActive(false);
            else 
                InstructionsPopup = instructionsPopup;
        }

        protected virtual void Start() => GetComponent<Button>().onClick.AddListener(ShowInstructions);
        public virtual void ShowInstructions() => InstructionsPopup.ShowInstructions();
    }
}
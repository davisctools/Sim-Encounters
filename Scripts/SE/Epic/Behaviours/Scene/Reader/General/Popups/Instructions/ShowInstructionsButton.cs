using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ShowInstructionsButton : MonoBehaviour
    {
        public Button Button { get => button; set => button = value; }
        [SerializeField] private Button button;

        protected BaseInstructionsPopup InstructionsPopup { get; set; }

        [Inject]
        public void Inject(BaseInstructionsPopup instructionsPopup)
            => InstructionsPopup = instructionsPopup;

        protected virtual void Awake() => Button.onClick.AddListener(ShowInstructions);
        public virtual void ShowInstructions() => InstructionsPopup.ShowInstructions();
    }
}
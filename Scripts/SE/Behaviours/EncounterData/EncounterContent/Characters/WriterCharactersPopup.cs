using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    public class WriterCharactersPopup : MonoBehaviour
    {
        public Button CancelButton { get => cancelButton; set => cancelButton = value; }
        [SerializeField] private Button cancelButton;
        public Button ApplyButton { get => applyButton; set => applyButton = value; }
        [SerializeField] private Button applyButton;


        public WriterCharactersDrawer CharactersDrawer { get => charactersDrawer; set => charactersDrawer = value; }
        [SerializeField] private WriterCharactersDrawer charactersDrawer;
    }
}
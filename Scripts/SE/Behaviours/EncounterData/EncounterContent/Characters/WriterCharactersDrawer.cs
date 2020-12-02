using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    public class WriterCharactersDrawer : MonoBehaviour
    {

        public GameObject CharactersParent { get => charactersParent; set => charactersParent = value; }
        [SerializeField] private GameObject charactersParent;

        public Button AddButton { get => addButton; set => addButton = value; }
        [SerializeField] private Button addButton;


        public WriterCharacter CharacterPrefab { get => characterPrefab; set => characterPrefab = value; }
        [SerializeField] private WriterCharacter characterPrefab;

    }
}
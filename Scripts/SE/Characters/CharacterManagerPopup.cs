using ClinicalTools.Collections;
using ClinicalTools.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class CharacterManagerPopup : MonoBehaviour
    {
        [SerializeField] private Button applyButton;
        [SerializeField] private Button addCharacterButton;

        public BaseRearrangeableGroup ReorderableGroup { get => reorderableGroup; set => reorderableGroup = value; }
        [SerializeField] private BaseRearrangeableGroup reorderableGroup;

        protected CharacterEditor.Factory CharacterEditorFactory { get; set; }
        [Inject]
        public virtual void Inject(CharacterEditor.Factory characterEditorFactory)
            => CharacterEditorFactory = characterEditorFactory;

        protected OrderedCollection<Character> Characters { get; set; }
        protected Dictionary<Character, CharacterEditor> CharacterEditors { get; } = new Dictionary<Character, CharacterEditor>();

        protected virtual void Start() => addCharacterButton.onClick.AddListener(AddCharacter);

        protected virtual void AddCharacter() => AddCharacterEditor(new Character());

        protected virtual void EditCharacters(OrderedCollection<Character> characters)
        {
            gameObject.SetActive(true);

            Characters = characters;
            foreach (var characterEditor in CharacterEditors.Values) {
                ReorderableGroup.Remove(characterEditor);
                Destroy(characterEditor);
            }

            CharacterEditors.Clear();
            foreach (var character in Characters.Values)
                AddCharacterEditor(character);
        }

        protected virtual void AddCharacterEditor(Character character)
        {
            var characterEditor = CharacterEditorFactory.Create();
            characterEditor.transform.SetParent(ReorderableGroup.transform);
            characterEditor.Display(character);
            CharacterEditors.Add(character, characterEditor);
            ReorderableGroup.Add(characterEditor);
        }

        protected virtual void Serialize()
        {
            Dictionary<int, Character> characterPositions = new Dictionary<int, Character>();
            foreach (var characterEditor in CharacterEditors) {
                characterPositions.Add(characterEditor.Value.transform.GetSiblingIndex(), characterEditor.Key);
                characterEditor.Value.Serialize();
                if (!Characters.Contains(characterEditor.Key))
                    Characters.Add(characterEditor.Key);
            }

            for (int i = 0; i < CharacterEditors.Count - 1; i++)
                Characters.MoveValue(i, Characters.IndexOf(characterPositions[i]));
        }

        protected virtual void Apply()
        {
            Serialize();
            gameObject.SetActive(false);
        }

        protected virtual void Cancel()
        {
            gameObject.SetActive(false);
        }
    }
}
using ClinicalTools.Collections;
using ClinicalTools.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class CharacterManagerPopup : BaseCharactersEditor, ICloseHandler
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

        protected virtual void Start()
        {
            addCharacterButton.onClick.AddListener(AddCharacter);
            applyButton.onClick.AddListener(Apply);
        }
        protected virtual void AddCharacter() => AddCharacterEditor(new Character());

        public override void EditCharacters(OrderedCollection<Character> characters)
        {
            gameObject.SetActive(true);

            Characters = characters;
            foreach (var characterEditor in CharacterEditors.Values) {
                ReorderableGroup.Remove(characterEditor);
                Destroy(characterEditor.gameObject);
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
            foreach (var characterEditor in CharacterEditors) {
                var character = characterEditor.Key;
                var editor = characterEditor.Value;
                if (editor == null) {
                    Characters.Remove(character);
                    continue;
                }

                characterEditor.Value.Serialize();
                if (!Characters.Contains(characterEditor.Key))
                    Characters.Add(characterEditor.Key);
                Characters.MoveValue(editor.transform.GetSiblingIndex(), Characters.IndexOf(character));
            }
        }

        protected virtual void Apply()
        {
            Serialize();
            gameObject.SetActive(false);
        }

        public virtual void Close(object sender) => gameObject.SetActive(false);
    }
}
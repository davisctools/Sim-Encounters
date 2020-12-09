using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    [RequireComponent(typeof(Button))]
    public class EditCharactersButton : MonoBehaviour
    {
        protected Button Button { get; set; }

        protected ISelectedListener<EncounterSelectedEventArgs> EncounterSelectedListener { get; set; }
        protected BaseCharactersEditor CharactersEditor { get; set; }
        [Inject]
        public void Inject(ISelectedListener<EncounterSelectedEventArgs> encounterSelectedListener, BaseCharactersEditor charactersEditor)
        {
            EncounterSelectedListener = encounterSelectedListener;
            CharactersEditor = charactersEditor;
        }

        protected virtual void Start()
        {
            Button = GetComponent<Button>();
            Button.interactable = false;
            EncounterSelectedListener.Selected += OnEncounterSelected;
            if (EncounterSelectedListener.CurrentValue != null)
                OnEncounterSelected(this, EncounterSelectedListener.CurrentValue);
        }

        protected virtual void OnEncounterSelected(object sender, EncounterSelectedEventArgs e)
        {
            Button.interactable = true;
            Button.onClick.RemoveAllListeners();
            Button.onClick.AddListener(EditCharacters);
        }

        protected virtual void EditCharacters() => CharactersEditor.EditCharacters(EncounterSelectedListener.CurrentValue.Encounter.Content.NonImageContent.Characters);
    }
}
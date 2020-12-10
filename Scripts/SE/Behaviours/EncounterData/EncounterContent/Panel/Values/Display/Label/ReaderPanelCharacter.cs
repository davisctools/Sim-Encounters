using ClinicalTools.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ReaderPanelCharacter : MonoBehaviour
    {
        [SerializeField] private string valueName = null;
        protected virtual string Name => !string.IsNullOrWhiteSpace(valueName) ? valueName : name;

        public List<Image> PrimaryColoredObjects { get => primaryColoredObjects; set => primaryColoredObjects = value; }
        [SerializeField] private List<Image> primaryColoredObjects;
        public List<Image> SecondaryColoredObjects { get => secondaryColoredObjects; set => secondaryColoredObjects = value; }
        [SerializeField] private List<Image> secondaryColoredObjects;
        public List<Image> CharacterIconSprites { get => characterIconSprites; set => characterIconSprites = value; }
        [SerializeField] private List<Image> characterIconSprites;


        protected IIconSpriteRetriever IconSpriteRetriever { get; set; }
        protected ISelectedListener<EncounterSelectedEventArgs> EncounterSelectedListener { get; set; }
        protected ISelectedListener<PanelSelectedEventArgs> PanelSelectedListener { get; set; }
        [Inject]
        public virtual void Inject(
            IIconSpriteRetriever iconSpriteRetriever,
            ISelectedListener<EncounterSelectedEventArgs> encounterSelectedListener,
            ISelectedListener<PanelSelectedEventArgs> panelSelectedListener)
        {
            IconSpriteRetriever = iconSpriteRetriever;
            EncounterSelectedListener = encounterSelectedListener;
            PanelSelectedListener = panelSelectedListener;
        }

        protected virtual OrderedCollection<Character> Characters => EncounterSelectedListener.CurrentValue.Encounter.Content.NonImageContent.Characters;

        protected virtual void Start()
        {
            PanelSelectedListener.Selected += OnPanelSelected;
            if (PanelSelectedListener.CurrentValue != null)
                OnPanelSelected(this, PanelSelectedListener.CurrentValue);
        }

        protected virtual void OnPanelSelected(object sender, PanelSelectedEventArgs e)
        {
            var values = e.Panel.Values;
            if (!values.ContainsKey(Name)) {
                return;
            }

            var character = Characters[values[Name]];
            foreach (var primaryColoredObject in PrimaryColoredObjects)
                primaryColoredObject.color = character.PrimaryColor;
            foreach (var secondaryColoredObject in SecondaryColoredObjects)
                secondaryColoredObject.color = character.SecondaryColor;
            
            if (CharacterIconSprites.Count == 0)
                return;

            var sprite = IconSpriteRetriever.GetIconSprite(EncounterSelectedListener.CurrentValue.Encounter, character.Icon);
            foreach (var characterIconSprite in CharacterIconSprites) {
                characterIconSprite.color = character.Icon.Color;
                characterIconSprite.sprite = sprite;
            }
        }

    }
}
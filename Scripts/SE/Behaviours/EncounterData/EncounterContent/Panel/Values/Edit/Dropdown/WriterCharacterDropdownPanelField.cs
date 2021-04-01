using ClinicalTools.Collections;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    [RequireComponent(typeof(TMP_Dropdown))]
    public class WriterCharacterDropdownPanelField : BaseWriterPanelField
    {
        [SerializeField] private string valueName = null;
        public override string Name => !string.IsNullOrWhiteSpace(valueName) ? valueName : name;

        public override string Value 
            => Dropdown.value >= 0 && Dropdown.value < CharacterKeys.Length ? CharacterKeys[Dropdown.value] : null;

        protected TMP_Dropdown Dropdown => (dropdown == null) ? dropdown = GetComponent<TMP_Dropdown>() : dropdown;
        private TMP_Dropdown dropdown;

        protected IIconSpriteRetriever IconSpriteRetriever { get; set; }
        protected ISelectedListener<EncounterSelectedEventArgs> EncounterSelectedListener { get; set; }
        [Inject]
        public virtual void Inject(IIconSpriteRetriever iconSpriteRetriever,
            ISelectedListener<EncounterSelectedEventArgs> encounterSelectedListener)
        {
            IconSpriteRetriever = iconSpriteRetriever;
            EncounterSelectedListener = encounterSelectedListener;
        }

        protected virtual void Start()
        {
            Started = true;
            EncounterSelectedListener.Selected += OnEncounterSelected;
            if (EncounterSelectedListener.CurrentValue != null)
                OnEncounterSelected(this, EncounterSelectedListener.CurrentValue);
        }

        protected bool Started { get; set; }
        protected string InitialValue { get; set; }
        protected string[] CharacterKeys { get; set; }
        protected OrderedCollection<Character> Characters { get; set; }

        protected virtual void OnEncounterSelected(object sender, EncounterSelectedEventArgs e)
        {
            Characters = e.Encounter.Content.Characters;
            Dropdown.ClearOptions();
            CharacterKeys = new string[Characters.Count];
            var options = new List<TMP_Dropdown.OptionData>();
            for (int i = 0; i < Characters.Count; i++) {
                var character = Characters[i];
                CharacterKeys[i] = character.Key;
                var sprite = IconSpriteRetriever.GetIconSprite(e.Encounter, character.Value.Icon);
                options.Add(new TMP_Dropdown.OptionData(character.Value.Role, sprite));
            }
            Dropdown.AddOptions(options);

            if (InitialValue != null)
                SetValue(InitialValue);
        }


        protected string LegacyName => "characterName";
        protected override void OnPanelSelected(object sender, PanelSelectedEventArgs e)
        {
            var values = e.Panel.Values;
            string value;
            if (values.ContainsKey(Name)) {
                value = values[Name];
            } else if (values.ContainsKey(LegacyName)) {
                value = values[LegacyName];
            } else {
                Dropdown.value = 0;
                return;
            }

            if (Started)
                SetValue(value);
            else
                InitialValue = value;
        }

        protected virtual void SetValue(string key)
        {
            for (int i = 0; i < CharacterKeys.Length; i++) {
                if (!key.Equals(CharacterKeys[i], StringComparison.InvariantCultureIgnoreCase))
                    continue;

                Dropdown.value = i;
                break;
            }
        }
    }
}
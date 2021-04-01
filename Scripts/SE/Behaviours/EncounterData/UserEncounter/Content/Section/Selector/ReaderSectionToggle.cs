using ClinicalTools.UI;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ReaderSectionToggle : MonoBehaviour
    {
        public SelectableToggle SelectToggle { get => selectToggle; set => selectToggle = value; }
        [SerializeField] private SelectableToggle selectToggle;
        public Image Image { get => image; set => image = value; }
        [SerializeField] private Image image;
        public Image Icon { get => icon; set => icon = value; }
        [SerializeField] private Image icon;
        public TextMeshProUGUI NameLabel { get => nameLabel; set => nameLabel = value; }
        [SerializeField] private TextMeshProUGUI nameLabel;
        public GameObject Visited { get => visited; set => visited = value; }
        [SerializeField] private GameObject visited;

        public Action Selected;
        public Action Unselected;

        protected virtual void Awake() => Initialize();

        private bool initialized = false;
        protected virtual void Initialize()
        {
            if (initialized)
                return;

            initialized = true;
            SelectToggle.Selected += () => Selected?.Invoke();
            Selected += () => SetColor(true);
            SelectToggle.Unselected += () => Unselected?.Invoke();
            Unselected += () => SetColor(false);
        }

        protected UserSection UserSection { get; set; }
        public void Display(UserSection userSection)
        {
            Initialize();

            if (UserSection != null)
                userSection.StatusChanged -= StatusChanged;

            UserSection = userSection;
            var section = userSection.Data;
            SetColor(false);

            NameLabel.text = userSection.Data.Name;
            Visited.SetActive(userSection.IsRead());
            userSection.StatusChanged += StatusChanged;

            if (Icon == null)
                return;

            var icons = userSection.Encounter.Data.Content.Icons;
            if (section.IconKey != null && icons.ContainsKey(section.IconKey))
                Icon.sprite = icons[section.IconKey];
        }

        private void StatusChanged() => Visited.SetActive(UserSection.IsRead());
        public void Select()
        {
            SelectToggle.SelectWithNoNotify();
            SetColor(true);
        }
        public void Deselect()
        {
            SelectToggle.DeselectWithNoNotify();
            SetColor(false);
        }
        public void SetToggleGroup(ToggleGroup group) => SelectToggle.SetToggleGroup(group);

        protected virtual void SetColor(bool isOn)
        {
            Color foregroundColor, backgroundColor;
            if (isOn) {
                foregroundColor = Color.white;
                backgroundColor = UserSection.Data.Color;
            } else {
                foregroundColor = UserSection.Data.Color;
                backgroundColor = Color.white;
            }

            Image.color = backgroundColor;
            if (Icon != null)
                Icon.color = foregroundColor;
            NameLabel.color = foregroundColor;
        }


        public class Factory : PlaceholderFactory<ReaderSectionToggle> { }
    }
}

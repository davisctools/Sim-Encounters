using ClinicalTools.UI.Extensions;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.UI
{
    public class ColorUI : BaseColorEditor
    {
        protected Transform ColorTogglesParent { get => colorTogglesParent; set => colorTogglesParent = value; }
        [SerializeField] private Transform colorTogglesParent;
        public List<BaseColorEditor> ColorEditors { get => colorEditors; set => colorEditors = value; }
        [SerializeField] private List<BaseColorEditor> colorEditors;
        public Image CustomColorImage { get => customColorImage; set => customColorImage = value; }
        [SerializeField] private Image customColorImage;
        protected virtual Toggle CustomColorToggle { get; set; }

        public override event Action<Color> ValueChanged;

        public override Color GetValue() => SelectedImage.color;
        protected Image SelectedImage { get; set; }

        protected virtual void Awake()
        {
            CustomColorToggle = CustomColorImage.GetComponent<Toggle>();

            foreach (var colorEditor in ColorEditors)
                colorEditor.ValueChanged += Display;

            var colorToggles = ColorTogglesParent.GetComponentsInChildren<Toggle>();
            foreach (var colorToggle in colorToggles)
                colorToggle.AddOnSelectListener(() => ColorToggleSelected(colorToggle));
            ColorToggleSelected(colorToggles[0]);


            Display(CustomColorImage.color);
        }

        private void ColorToggleSelected(Toggle toggle)
        {
            var toggleImage = toggle.GetComponent<Image>();
            SelectedImage = toggleImage;
            ValueChanged?.Invoke(GetValue());
        }

        public override void Display(Color color)
        {
            foreach (var colorEditor in ColorEditors)
                colorEditor.Display(color);

            CustomColorImage.color = color;
            CustomColorToggle.isOn = true;
        }
    }
}
using ClinicalTools.UI;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    [RequireComponent(typeof(Button))]
    public class ColorSelectionPopupButton : BaseColorEditor
    {
        [SerializeField] private Image colorPreview;

        public override event Action<Color> ValueChanged;

        protected Color Color { get; set; }

        protected BaseColorSelector ColorSelectionPopup { get; set; }
        [Inject] public virtual void Inject(BaseColorSelector colorSelectionPopup) => ColorSelectionPopup = colorSelectionPopup;

        protected virtual void Start() => GetComponent<Button>().onClick.AddListener(OnButtonClicked);
        protected virtual void OnButtonClicked() => ColorSelectionPopup.SelectColor(Color).AddOnCompletedListener(OnColorSelected);

        protected virtual void OnColorSelected(TaskResult<Color> colorResult)
        {
            if (colorResult.HasValue())
                SetColor(colorResult.Value);
        }

        public override void Display(Color color) => SetColor(color);
        protected virtual void SetColor(Color color)
        {
            Color = color;
            colorPreview.color = color;
            ValueChanged?.Invoke(color);
        }

        public override Color GetValue() => Color;
    }
}
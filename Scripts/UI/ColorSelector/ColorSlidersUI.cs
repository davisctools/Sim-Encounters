using System;
using UnityEngine;

namespace ClinicalTools.UI
{
    public class ColorSlidersUI : BaseColorEditor
    {
        public ValueSliderUI Red { get => red; set => red = value; }
        [SerializeField] private ValueSliderUI red;
        public ValueSliderUI Green { get => green; set => green = value; }
        [SerializeField] private ValueSliderUI green;
        public ValueSliderUI Blue { get => blue; set => blue = value; }
        [SerializeField] private ValueSliderUI blue;

        protected Color Value { get; set; }
        public override Color GetValue() => Value;
        public override event Action<Color> ValueChanged;

        protected virtual void Awake()
        {
            Red.ValueChanged += RedChanged;
            Green.ValueChanged += GreenChanged;
            Blue.ValueChanged += BlueChanged;
        }

        public override void Display(Color color)
        {
            if (Value == color)
                return;

            Red.Display(color.r);
            Green.Display(color.g);
            Blue.Display(color.b);

            Value = color;
        }

        protected virtual void RedChanged(float value)
        {
            if (value == Value.r)
                return;

            var color = Value;
            color.r = value;
            SetValue(color);
        }
        protected virtual void GreenChanged(float value)
        {
            if (value == Value.g)
                return;

            var color = Value;
            color.g = value;
            SetValue(color);
        }
        protected virtual void BlueChanged(float value)
        {
            if (value == Value.b)
                return;

            var color = Value;
            color.b = value;
            SetValue(color);
        }

        protected void SetValue(Color color)
        {
            Value = color;
            ValueChanged?.Invoke(color);
        }
    }
}
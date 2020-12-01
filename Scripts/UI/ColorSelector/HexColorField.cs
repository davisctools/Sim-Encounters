using System;
using TMPro;
using UnityEngine;

namespace ClinicalTools.UI
{
    public class HexColorField : BaseColorEditor
    {
        public TMP_InputField HexField { get => hexField; set => hexField = value; }
        [SerializeField] private TMP_InputField hexField;

        protected virtual HexStringConverter HexStringConverter { get; } = new HexStringConverter();

        protected Color Value { get; set; }
        public override Color GetValue() => Value;
        public override event Action<Color> ValueChanged;

        protected virtual void Awake()
        {
            HexField.onValueChanged.AddListener(FieldChanged);
        }

        public override void Display(Color color)
        {
            Value = color;

            UpdateField(color);
        }

        protected string HexText { get; set; }
        private void UpdateField(Color color)
        {
            var redHexString = HexStringConverter.FloatToHex(color.r);
            var greenHexString = HexStringConverter.FloatToHex(color.g);
            var blueHexString = HexStringConverter.FloatToHex(color.b);
            HexText = redHexString + greenHexString + blueHexString;

            HexField.text = HexText;
        }

        private const int RED_START_INDEX = 0;
        private const int GREEN_START_INDEX = 2;
        private const int BLUE_START_INDEX = 4;
        private const int VALUE_HEX_LENGTH = 2;
        private const int COLOR_HEX_LENGTH = 6;
        protected virtual void FieldChanged(string text)
        {
            if (text == HexText)
                return;

            HexText = GetHexText(text);
            if (text != HexText)
                HexField.text = HexText;

            if (HexText.Length != COLOR_HEX_LENGTH)
                return;

            var redHexString = text.Substring(RED_START_INDEX, VALUE_HEX_LENGTH);
            var greenHexString = text.Substring(GREEN_START_INDEX, VALUE_HEX_LENGTH);
            var blueHexString = text.Substring(BLUE_START_INDEX, VALUE_HEX_LENGTH);

            var value = Value;
            value.r = HexStringConverter.HexToFloat(redHexString);
            value.g = HexStringConverter.HexToFloat(greenHexString);
            value.b = HexStringConverter.HexToFloat(blueHexString);

            ValueChanged?.Invoke(value);
        }

        protected virtual string GetHexText(string text)
        {
            var newText = "";

            foreach (var ch in text) {
                if (HexStringConverter.IsHex(ch))
                    newText += ch;

                if (newText.Length == COLOR_HEX_LENGTH)
                    break;
            }

            return newText;
        }
    }
}
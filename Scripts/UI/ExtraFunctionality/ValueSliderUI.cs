using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.UI
{
    public class ValueSliderUI : MonoBehaviour
    {
        public Slider Slider { get => slider; set => slider = value; }
        [SerializeField] private Slider slider;
        public TextMeshProUGUI ValueLabel { get => valueLabel; set => valueLabel = value; }
        [SerializeField] private TextMeshProUGUI valueLabel;

        protected float Value { get; set; }
        public event Action<float> ValueChanged;


        protected void Awake()
        {
            Slider.onValueChanged.AddListener(SliderChanged);
        }

        public void Display(float value)
        {
            Value = value;

            Slider.value = Value;
            UpdateLabel();
        }

        protected void SliderChanged(float value)
        {
            if (value == Value)
                return;
            Value = value;

            UpdateLabel();

            ValueChanged?.Invoke(value);
        }

        protected int VALUE_LABEL_MAX = 255;
        protected void UpdateLabel()
        {
            ValueLabel.text = Mathf.RoundToInt(Value * VALUE_LABEL_MAX).ToString();
        }
    }
}
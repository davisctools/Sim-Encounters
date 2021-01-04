using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.UI
{
    [RequireComponent(typeof(Slider))]
    public class ResizeSlider : MonoBehaviour
    {
        protected Slider Slider => (slider == null) ? slider = GetComponent<Slider>() : slider;
        private Slider slider;

        private float currentResizeValue;
        protected virtual void Awake()
        {
            Slider.value = CanvasResizer.ResizeValue01;
            currentResizeValue = CanvasResizer.ResizeValue01;
            Slider.onValueChanged.AddListener(SliderValueChanged);
        }

        protected virtual void SliderValueChanged(float value)
            => CanvasResizer.ResizeValue01 = value;

        protected virtual void OnEnable() => Update();

        protected virtual void Update()
        {
            if (currentResizeValue == CanvasResizer.ResizeValue01)
                return;

            Slider.value = CanvasResizer.ResizeValue01;
            currentResizeValue = CanvasResizer.ResizeValue01;
        }
    }
}
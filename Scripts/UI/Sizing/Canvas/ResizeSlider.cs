using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.UI
{
    [RequireComponent(typeof(Slider))]
    public class ResizeSlider : MonoBehaviour
    {
        protected Slider Slider => (slider == null) ? slider = GetComponent<Slider>() : slider;
        private Slider slider;

        private float currentResizeValue;

        protected CanvasResizer CanvasResizer { get; set; }
        [Inject] public virtual void Inject(CanvasResizer canvasResizer)
        {
            CanvasResizer = canvasResizer;
            Slider.value = CanvasResizer.ResizeValue01;
            currentResizeValue = CanvasResizer.ResizeValue01;
            Slider.onValueChanged.AddListener(SliderValueChanged);
        }

        protected virtual void SliderValueChanged(float value)
            => CanvasResizer.ResizeValue01 = value;

        protected virtual void OnEnable() => Update();

        protected virtual void Update()
        {
            if (CanvasResizer == null || currentResizeValue == CanvasResizer.ResizeValue01)
                return;

            Slider.value = CanvasResizer.ResizeValue01;
            currentResizeValue = CanvasResizer.ResizeValue01;
        }
    }
}
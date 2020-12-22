using ClinicalTools.SEColors;
using UnityEngine;

namespace ClinicalTools.UI
{
    [ExecuteAlways]
    public abstract class ColorBehaviour : MonoBehaviour
    {
        public ColorType ColorType { get => colorType; set => colorType = value; }
        [SerializeField] private ColorType colorType;

        protected virtual IColorManager ColorManager { get; } = new ColorManager();

        protected virtual void Start() => UpdateColor();
        protected virtual void Update()
        {
            if (previousType != ColorType)
                UpdateColor();
        }

        private ColorType previousType;
        protected virtual void UpdateColor()
        {
            previousType = ColorType;
            SetColor(ColorManager.GetColor(ColorType));
        }

        protected abstract void SetColor(Color color);
    }
}
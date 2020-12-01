﻿using UnityEngine;
using Zenject;

namespace ClinicalTools.UI
{
    [ExecuteAlways]
    public abstract class ColorBehaviour : MonoBehaviour
    {
        public ColorType ColorType { get => colorType; set => colorType = value; }
        [SerializeField] private ColorType colorType;

        protected IColorManager ColorManager { get; set; }
        [Inject] public virtual void Inject(IColorManager colorManager) => ColorManager = colorManager;

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
            if (ColorManager == null)
                ColorManager = new ColorManager();
            SetColor(ColorManager.GetColor(ColorType));
        }

        protected abstract void SetColor(Color color);
    }
}
using System;
using System.Collections;
using System.Collections.Generic;

namespace ClinicalTools.UI
{
    public class SizeManager
    {
        private float size;
        public float Size {
            get => size;
            set {
                size = value;
                Resized?.Invoke(size);
            }
        }
        public event Action<float> Resized;
    }
}
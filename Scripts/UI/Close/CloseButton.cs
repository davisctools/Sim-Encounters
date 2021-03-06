﻿using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.UI
{
    [RequireComponent(typeof(Button))]
    public class CloseButton : MonoBehaviour
    {
        protected Button Button => (button == null) ? button = GetComponent<Button>() : button;
        private Button button;

        protected ICloseHandler CloseHandler { get; set; }
        [Inject] public virtual void Inject(ICloseHandler closeHandler) => CloseHandler = closeHandler;

        protected virtual void Awake() => Button.onClick.AddListener(Close);
        protected virtual void Close() => CloseHandler?.Close(this);
    }
}
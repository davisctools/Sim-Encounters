using ClinicalTools.UI;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    public class ColorSelectionPopup : BaseColorSelector, ICloseHandler
    {
        [SerializeField] private BaseColorEditor colorEditor;
        [SerializeField] private Button applyButton;

        protected virtual void Awake() => applyButton.onClick.AddListener(Apply);
        protected WaitableTask<Color> CurrentColorTask { get; set; }
        public override WaitableTask<Color> SelectColor(Color color)
        {
            CurrentColorTask?.SetError(new Exception("New popup opened"));
            CurrentColorTask = new WaitableTask<Color>();

            gameObject.SetActive(true);

            colorEditor.Display(color);

            return CurrentColorTask;
        }
        protected virtual void Apply()
        {
            CurrentColorTask.SetResult(colorEditor.GetValue());
            CurrentColorTask = null;
            gameObject.SetActive(false);
        }
        public virtual void Close(object sender)
        {
            CurrentColorTask.SetError(new Exception("Could not set result."));
            CurrentColorTask = null;
            gameObject.SetActive(false);
        }
    }
}
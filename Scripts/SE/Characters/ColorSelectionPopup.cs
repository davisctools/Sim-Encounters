using ClinicalTools.UI;
using System;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class ColorSelectionPopup : MonoBehaviour
    {
        [SerializeField] private BaseColorEditor colorEditor;

        protected WaitableTask<Color> CurrentColorTask { get; set; }
        public virtual WaitableTask<Color> SelectColor(Color color)
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
            gameObject.SetActive(false);
        }
        protected virtual void Cancel()
        {
            CurrentColorTask.SetError(new Exception("Could not set result."));
            gameObject.SetActive(false);
        }
    }
}
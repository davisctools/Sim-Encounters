using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ReaderPanelShowObjectIfCharacter : MonoBehaviour
    {
        protected ISelectedListener<PanelSelectedEventArgs> PanelSelectedListener { get; set; }
        [Inject]
        public virtual void Inject(ISelectedListener<PanelSelectedEventArgs> panelSelectedListener)
        {
            PanelSelectedListener = panelSelectedListener;
            PanelSelectedListener.Selected += OnPanelSelected;
            if (PanelSelectedListener.CurrentValue != null)
                OnPanelSelected(PanelSelectedListener, PanelSelectedListener.CurrentValue);
        }

        protected virtual void OnDestroy() => PanelSelectedListener.Selected -= OnPanelSelected;
        protected virtual void OnPanelSelected(object sender, PanelSelectedEventArgs eventArgs)
            => gameObject.SetActive(ShouldShow(eventArgs.Panel.Values));
         
        private const string CharacterNameValueKey = "characterName";
        protected virtual bool ShouldShow(IDictionary<string, string> values)
            => values.ContainsKey(CharacterNameValueKey) 
            && values[CharacterNameValueKey].Equals(name, StringComparison.InvariantCultureIgnoreCase);
    }
}
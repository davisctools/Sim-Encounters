using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseWriterPanelField : MonoBehaviour, IWriterPanelField
    {
        public virtual string Name => name;
        public abstract string Value { get; }

        protected ISelectedListener<PanelSelectedEventArgs> PanelSelectedListener { get; set; }
        [Inject]
        public virtual void Inject(ISelectedListener<PanelSelectedEventArgs> panelSelectedListener)
        {
            PanelSelectedListener = panelSelectedListener;
            PanelSelectedListener.Selected += OnPanelSelected;
            if (PanelSelectedListener.CurrentValue != null)
                OnPanelSelected(this, PanelSelectedListener.CurrentValue);
        }

        protected abstract void OnPanelSelected(object sender, PanelSelectedEventArgs e);
    }
}
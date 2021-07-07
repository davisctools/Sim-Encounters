using System;

namespace ClinicalTools.SimEncounters
{
    public abstract class ReaderOptionPanelBehaviour : BaseReaderPanelBehaviour
    {
        public abstract event Action SelectChanged;

        public virtual void GetFeedback() => CurrentPanel.SetChildPanelsRead(true);
    }
}
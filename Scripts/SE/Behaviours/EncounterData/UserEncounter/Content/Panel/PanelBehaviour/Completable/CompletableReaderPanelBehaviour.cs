using System;

namespace ClinicalTools.SimEncounters
{
    public abstract class CompletableReaderPanelBehaviour : BaseReaderPanelBehaviour, IPanelCompletedHandler
    {
        public bool IsCompleted { get; protected set; } = false;
        public virtual event Action Completed;
        public virtual void SetCompleted()
        {
            CurrentPanel.SetChildPanelsRead(true);
            IsCompleted = true;
            Completed?.Invoke();
        }
    }
}
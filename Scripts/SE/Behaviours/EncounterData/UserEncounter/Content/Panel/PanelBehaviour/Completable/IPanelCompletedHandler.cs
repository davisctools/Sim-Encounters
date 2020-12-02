using System;

namespace ClinicalTools.SimEncounters
{
    public interface IPanelCompletedHandler
    {
        event Action Completed;
        bool IsCompleted { get; }
        void SetCompleted();
    }
}
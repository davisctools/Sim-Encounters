using System;

namespace ClinicalTools.SimEncounters
{
    public class CompletionHandler : ICompletionHandler
    {
        public event Action Completed;
        public virtual void Complete() => Completed?.Invoke();
    }
}
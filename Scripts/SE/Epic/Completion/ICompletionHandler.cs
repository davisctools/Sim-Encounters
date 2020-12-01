using System;

namespace ClinicalTools.SimEncounters
{
    public interface ICompletionHandler
    {
        event Action Completed;
        void Complete();
    }
}
using System;

namespace ClinicalTools.SimEncounters
{
    public interface ISelectedListener<T>
        where T : EventArgs
    {
        T CurrentValue { get; }
        event SelectedHandler<T> Selected;
    }
}
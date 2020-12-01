using System;

namespace ClinicalTools.SimEncounters
{
    public delegate void SelectedHandler<T>(object sender, T e);
    public interface ISelector<T> : ISelectedListener<T>
        where T : EventArgs
    {
        void Select(object sender, T eventArgs);
    }
}
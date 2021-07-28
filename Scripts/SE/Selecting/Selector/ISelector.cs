using System;

namespace ClinicalTools.SimEncounters
{
    public delegate void SelectedHandler<T>(object sender, T e);

    // This interface will practically always be used in conjunction with ISelectedHandler
    // This should generally not be used if it wouldn't make sense for children objects to select the value
    // The main exception would be in error checking children's calls before passing it up to the actual Selector
    public interface ISelector<T>
        where T : EventArgs
    {
        void Select(object sender, T eventArgs);
    }
}
using System;

namespace ClinicalTools.SimEncounters
{
    public interface ILinearEncounterNavigator
    {
        event SelectedHandler<UserTabSelectedEventArgs> EncounterTabPositionChanged;
        UserTabSelectedEventArgs CurrentTab { get; }

        bool HasNext();
        void GoToNext();
        bool HasPrevious();
        void GoToPrevious();
    }
}
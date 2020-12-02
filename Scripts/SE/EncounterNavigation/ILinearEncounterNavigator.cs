namespace ClinicalTools.SimEncounters
{
    public interface ILinearEncounterNavigator
    {
        bool HasNext();
        void GoToNext();
        bool HasPrevious();
        void GoToPrevious();
    }
}